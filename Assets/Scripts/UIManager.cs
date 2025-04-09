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
    public Button SimonClue;
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
        exitButton.onClick.AddListener(ExitToMainMenu);
        SimonClue.onClick.AddListener(ShowSimonClue);
        soundSlider.onValueChanged.AddListener(HandleVolumeChange);
        soundSlider.value = AudioListener.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SkipTyping();
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
        rect.sizeDelta = new Vector2(50, 50);
        
        // Add event trigger for hover effects
        EventTrigger trigger = clueObj.AddComponent<EventTrigger>();
        
        // Add pointer enter event
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        trigger.triggers.Add(enterEntry);
        
        // Add pointer exit event
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        trigger.triggers.Add(exitEntry);
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
                yield return new WaitForSeconds(0.05f);
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

    void ShowSimonClue() {
        if (tooltip.activeSelf) {
            tooltip.SetActive(false);
        }
        else if(clueCollected){
            tooltip.SetActive(true);
        }
    }
}
