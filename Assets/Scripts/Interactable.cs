using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    private GameObject UI;
    private TextMeshProUGUI dialogue;
    private GameObject dialogueBox;
    private GameObject interactIcon;
    private TextMeshProUGUI interactText;
    private TextMeshProUGUI skipText;
    public string dialogueText;
    private bool isInteractable = false;
    public bool isPaper = false;
    private bool hasBeenRead = false;
    private UIManager uiManager;
    private SpriteRenderer spriteRenderer;
    
    protected virtual void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        UI = player.transform.Find("UI")?.gameObject;
        dialogue = UI.transform.Find("Dialogue")?.GetComponent<TextMeshProUGUI>();
        dialogueBox = UI.transform.Find("DialogueBG").gameObject;
        interactIcon = UI.transform.Find("Interact").gameObject;
        interactText = UI.transform.Find("InteractText")?.GetComponent<TextMeshProUGUI>();
        skipText = UI.transform.Find("SkipText")?.GetComponent<TextMeshProUGUI>();
        uiManager = UI.GetComponent<UIManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void CleanupUI()
    {
        if (dialogueBox != null) dialogueBox.SetActive(false);
        if (interactIcon != null) interactIcon.SetActive(false);
        if (dialogue != null) dialogue.text = "";
        if (interactText != null) interactText.text = "";
        if (skipText != null) skipText.text = "";
        if (UI != null) UI.GetComponent<UIManager>().SkipTyping();
        isInteractable = false;
    }

    void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!dialogueBox.activeSelf)
                {
                    dialogueBox.SetActive(true);
                    UI.GetComponent<UIManager>().ShowDialogue(dialogueText);
                    interactIcon.SetActive(false);
                    interactText.text = "";
                    skipText.text = "Left Click to Skip";
                    
                    if (isPaper)
                    {
                        uiManager.clueCollected = true;
                    }
                }
                else if (!UI.GetComponent<UIManager>().IsTyping())
                {
                    dialogueBox.SetActive(false);
                    dialogue.text = "";
                    skipText.text = "";
                    
                    if (isPaper && spriteRenderer != null)
                    {
                        hasBeenRead = true;
                        uiManager.AddClue(spriteRenderer.sprite, dialogueText);
                        CleanupUI();
                        uiManager.ShowClues();
                        Destroy(gameObject);
                    }
                    else
                    {
                        interactIcon.SetActive(true);
                        interactText.text = "Interact";
                    }
                }
            }
            
            if (dialogueBox.activeSelf && Input.GetMouseButtonDown(0) && UI.GetComponent<UIManager>().IsTyping())
            {
                UI.GetComponent<UIManager>().SkipTyping();
                skipText.text = "Press E to Exit";
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactIcon.SetActive(true);
            interactText.text = "Interact";
            isInteractable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CleanupUI();
            if (isPaper && hasBeenRead)
            {
                uiManager.clueCollected = true;
                uiManager.ShowClues();
                Destroy(gameObject);
            }
        }
    }

    void OnDisable()
    {
        CleanupUI();
    }
}
