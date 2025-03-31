using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
    private float typingSpeed = 0.05f;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string fullText;
    public Slider soundSlider;
    private bool tooltipShown = false;
    public GameObject tooltip; // Reference to your Tooltip GameObject
    
    
    [Header("Audio")]
    public AudioClip buttonClickSound;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        
        gameUI.SetActive(true);
        menuUI.SetActive(false);
        menuButton.onClick.AddListener(Menu);
        returnMenu.onClick.AddListener(ReturnMenu);
        cluesButton.onClick.AddListener(ShowClues);
        exitButton.onClick.AddListener(ExitToMainMenu);
        soundSlider.onValueChanged.AddListener(HandleVolumeChange);
        soundSlider.value = AudioListener.volume;

        // Make sure tooltip starts hidden
        if (tooltip != null)
            tooltip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SkipTyping();
        }

        // Check if mouse is in the correct position
        bool isMouseInPosition = Input.mousePosition.x > 45 && Input.mousePosition.x < 110 && 
                               Input.mousePosition.y > 976 && Input.mousePosition.y < 1040;

        // Show/hide tooltip based on mouse position
        if (tooltip != null)
        {
            if (isMouseInPosition && !tooltipShown)
            {
                tooltip.SetActive(true);
                tooltipShown = true;
            }
            else if (!isMouseInPosition && tooltipShown)
            {
                tooltip.SetActive(false);
                tooltipShown = false;
            }
        }
    }

    public void AddClue(Sprite clueSprite, string clueText)
    {
        // Create a new image object for the clue
        GameObject clueObj = new GameObject("Clue");
        
        // Add Image component and set the sprite
        Image clueImage = clueObj.AddComponent<Image>();
        clueImage.sprite = clueSprite;
        clueImage.preserveAspect = true;
        
        // Set size of the clue icon
        RectTransform rect = clueObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(50, 50); // Adjust size as needed
        
        // Add event trigger for hover effects
        EventTrigger trigger = clueObj.AddComponent<EventTrigger>();
        
        // Add pointer enter event
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        //enterEntry.callback.AddListener((data) => { ShowTooltip(clueObj, clueText); });
        trigger.triggers.Add(enterEntry);
        
        // Add pointer exit event
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { HideTooltip(clueObj); });
        trigger.triggers.Add(exitEntry);
        
        // Store the tooltip text
    }
/*
    private void ShowTooltip(GameObject clueObj, string tooltipText)
    {
        while (Input.mousePosition.x > 45 && Input.mousePosition.x < 110 && Input.mousePosition.y > 976 && Input.mousePosition.y < 1040) {
            Debug.Log("Showing tooltip");
            GameObject tooltip = Instantiate(tooltipPrefab, clueObj.transform);
            tooltip.name = "Tooltip";
            tooltip.GetComponentInChildren<TextMeshProUGUI>().text = tooltipText;
            
            // Position the tooltip above the clue
            RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();
            tooltipRect.anchoredPosition = new Vector2(0, 60); // Adjust position as needed
        }
    }
*/
    private void HideTooltip(GameObject clueObj)
    {
        Transform tooltip = clueObj.transform.Find("Tooltip");
        if (tooltip)
        {
            Destroy(tooltip.gameObject);
        }
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

    void ShowClues() {
        if (clues.activeSelf)
            clues.SetActive(false);
        else
            clues.SetActive(true);
    }

    public void ShowDialogue(string text)
    {
        fullText = text;
        // Stop any existing typing coroutine
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
            
        // Start new typing coroutine
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogue.text = "";
        
        foreach (char c in text)
        {
            dialogue.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        isTyping = false;
    }

    // Optional: Skip to end of text if player clicks while typing
    public void SkipTyping()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
                
            dialogue.text = fullText;  // Show full text immediately
            isTyping = false;
        }
    }

    void HandleVolumeChange(float value)
    {
        AudioListener.volume = value;
    }

    void ExitToMainMenu()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        
        // Stop the background music
        if (PersistantGameManager.Instance != null)
        {
            PersistantGameManager.Instance.StopMusic();
        }
        
        SceneManager.LoadScene("Scenes/Main Menu");
    }
}
