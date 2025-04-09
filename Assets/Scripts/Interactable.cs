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
    private UIManager uiManager;
    private SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // If dialogue box is not active, show it
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
                // If dialogue box is active and text is fully displayed, close it
                else if (!UI.GetComponent<UIManager>().IsTyping())
                {
                    dialogueBox.SetActive(false);
                    dialogue.text = "";
                    skipText.text = "";
                    interactIcon.SetActive(true);
                    interactText.text = "Interact";

                    if (isPaper && spriteRenderer != null)
                    {
                        uiManager.AddClue(spriteRenderer.sprite, dialogueText);
                        Destroy(gameObject);
                        uiManager.ShowClues();
                    }
                }
            }
            
            // Handle left click for skipping text animation
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
            dialogueBox.SetActive(false);
            interactIcon.SetActive(false);
            UI.GetComponent<UIManager>().SkipTyping();
            dialogue.text = "";
            interactText.text = "";
            skipText.text = "";
            isInteractable = false;
            if (isPaper)
            {
                uiManager.clueCollected = true;
                Destroy(gameObject);
                uiManager.ShowClues();
            }
        }
    }
    // When interactable becomes inactive, if dialogue still open, close dialogue prompt
    void OnDisable() {
        if (dialogueBox.activeSelf == true) {
            interactIcon.SetActive(false);
            interactText.text = "";
            dialogueBox.SetActive(false);
            dialogue.text = "";
            skipText.text = "";
        }
    }
}
