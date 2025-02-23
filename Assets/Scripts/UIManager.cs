using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Button menuButton;
    public Button returnMenu;
    public Button cluesButton;
    public GameObject gameUI;
    public GameObject menuUI;
    public GameObject clues;
    public TextMeshProUGUI dialogue;
    private float typingSpeed = 0.05f;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string fullText;
    
    // Start is called before the first frame update
    void Start()
    {
        gameUI.SetActive(true);
        menuUI.SetActive(false);
        menuButton.onClick.AddListener(Menu);
        returnMenu.onClick.AddListener(ReturnMenu);
        cluesButton.onClick.AddListener(ShowClues);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SkipTyping();
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
}
