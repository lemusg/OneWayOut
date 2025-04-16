using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// Add this class at the top of the file, outside the UIManager class
[System.Serializable]
public class ClueData
{
    public Sprite sprite;
    public string text;
    
    public ClueData(Sprite sprite, string text)
    {
        this.sprite = sprite;
        this.text = text;
    }
}

public static class ClueManager
{
    public static List<ClueData> collectedClues = new List<ClueData>();
    
    public static void AddClue(Sprite sprite, string text)
    {
        // Check if clue already exists
        if (!collectedClues.Exists(c => c.text == text))
        {
            collectedClues.Add(new ClueData(sprite, text));
        }
    }
}

public class UIManager : MonoBehaviour
{
    public Button menuButton;
    public Button returnMenu;
    public Button cluesButton;
    public Button exitButton;
    public GameObject gameUI;
    public GameObject menuUI;
    public GameObject clues;
    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI skipText;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string fullText;
    public Slider soundSlider;
    //private bool tooltipShown = false;
    public GameObject tooltip;
    public bool clueCollected = false;
    
    [Header("Clue System")]
    public GameObject clueContainer; // Container for organizing clues vertically
    public float clueSpacing = 60f; // Spacing between clues
    private List<GameObject> collectedClues = new List<GameObject>();
    private GameObject activeClue; // Track which clue is being hovered
    
    [Header("Audio")]
    public AudioClip typingSound; // Add this field for the typing sound effect
    private AudioSource audioSource;
    private AudioSource typingAudioSource; // Separate audio source for typing sounds
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        typingAudioSource = gameObject.AddComponent<AudioSource>();
        typingAudioSource.volume = 0.3f;
        
        gameUI.SetActive(true);
        menuUI.SetActive(false);
        
        menuButton.onClick.AddListener(() => {
            Debug.Log("Menu button clicked!");
            Menu();
        });
        returnMenu.onClick.AddListener(ReturnMenu);
        exitButton.onClick.AddListener(ExitToMainMenu);
        soundSlider.onValueChanged.AddListener(HandleVolumeChange);
        soundSlider.value = AudioListener.volume;

        // Restore previously collected clues
        RestoreClues();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SkipTyping();
        }
    }

    private void RestoreClues()
    {
        // Clear existing clue objects
        foreach (GameObject clueObj in collectedClues)
        {
            if (clueObj != null)
            {
                Destroy(clueObj);
            }
        }
        collectedClues.Clear();

        // Restore clues from persistent storage
        foreach (ClueData clueData in ClueManager.collectedClues)
        {
            AddClue(clueData.sprite, clueData.text);
        }
    }

    public void AddClue(Sprite clueSprite, string clueText)
    {
        // Add to persistent storage
        ClueManager.AddClue(clueSprite, clueText);

        // Create a new image object for the clue
        GameObject clueObj = new GameObject("Clue");
        clueObj.transform.SetParent(clueContainer.transform, false);
        
        // Add Image component and set the sprite
        Image clueImage = clueObj.AddComponent<Image>();
        clueImage.sprite = clueSprite;
        clueImage.preserveAspect = true;
        
        // Set size of the clue icon
        RectTransform rect = clueObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(50, 50);
        
        // Position the clue based on how many we already have
        float yOffset = -collectedClues.Count * clueSpacing;
        rect.anchoredPosition = new Vector2(0, yOffset);
        
        // Add event trigger for hover effects
        EventTrigger trigger = clueObj.AddComponent<EventTrigger>();
        
        // Add pointer enter event
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => {
            activeClue = clueObj;
            tooltip.SetActive(true);
            // Find and update the text component in the tooltip
            TextMeshProUGUI tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
            if (tooltipText != null) {
                tooltipText.text = clueText;
            }
            // Update tooltip position based on the hovered clue
            RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
            RectTransform clueRect = clueObj.GetComponent<RectTransform>();
            
            // Convert the clue's position to screen space
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, clueObj.transform.position);
            // Convert screen point to local point in the tooltip's parent
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipRect.parent.GetComponent<RectTransform>(),
                screenPoint,
                null,
                out Vector2 localPoint
            );
            
            // Position the tooltip to the left of the clue
            tooltipRect.localPosition = new Vector3(
                localPoint.x - tooltipRect.rect.width + 20f,
                localPoint.y,
                0f
            );
        });
        trigger.triggers.Add(enterEntry);
        
        // Add pointer exit event
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => {
            if (activeClue == clueObj) {
                tooltip.SetActive(false);
                activeClue = null;
            }
        });
        trigger.triggers.Add(exitEntry);
        
        // Add to our list of collected clues
        collectedClues.Add(clueObj);
    }

    void Menu()
    {
        gameUI.SetActive(false);
        menuUI.SetActive(true);
    }

    void ReturnMenu()
    {
        menuUI.SetActive(false);
        gameUI.SetActive(true);
    }

    public void ShowClues() {
        if (clueCollected) {
            if (clues.activeSelf)
                clues.SetActive(false);
            else
                clues.SetActive(true);
        }
    }

    public bool IsTyping()
    {
        return isTyping;
    }

    public void ShowDialogue(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    public void SkipTyping()
    {
        if (isTyping && typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            if (typingAudioSource != null)
            {
                typingAudioSource.Stop(); // Stop typing sound when dialogue is skipped
            }
            TextMeshProUGUI dialogueText = transform.Find("Dialogue")?.GetComponent<TextMeshProUGUI>();
            if (dialogueText != null)
            {
                dialogueText.text = fullText;
                isTyping = false;
                skipText.text = "Press E to Exit";
            }
        }
    }

    private IEnumerator TypeText(string text)
    {
        TextMeshProUGUI dialogueText = transform.Find("Dialogue")?.GetComponent<TextMeshProUGUI>();
        if (dialogueText != null)
        {
            fullText = text;
            isTyping = true;
            dialogueText.text = "";
            foreach (char c in text)
            {
                dialogueText.text += c;
                if (typingSound != null && c != ' ') // Don't play sound for spaces
                {
                    typingAudioSource.PlayOneShot(typingSound, 0.3f);
                }
                yield return new WaitForSeconds(0.08f);
            }
            isTyping = false;
            skipText.text = "Press E to Exit";
        }
        typingCoroutine = null;
    }

    void HandleVolumeChange(float value)
    {
        AudioListener.volume = value;
    }

    void ExitToMainMenu()
    {
        // Stop the background music
        if (PersistantGameManager.Instance != null)
        {
            PersistantGameManager.Instance.StopMusic();
        }
        SceneManager.LoadScene("Scenes/Main Menu");
        boxTrigger.boxesInTrigger = 0;
        RotatePuzzle.correctRotations = 0;
        LightTile.correctTilesLit = 0;
        LightTile.incorrectTilesLit = 0;
    }

    void ShowSimonClue() {
        if (tooltip.activeSelf) {
            tooltip.SetActive(false);
        }
        else if(clueCollected){
            tooltip.SetActive(true);
        }
    }
}
