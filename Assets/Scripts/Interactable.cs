using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public GameObject UI;
    private TextMeshProUGUI dialogue;
    private GameObject dialogueBox;
    private GameObject interactIcon;
    private TextMeshProUGUI interactText;
    private TextMeshProUGUI skipText;
    public string dialogueText;
    private bool isInteractable = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogue = UI.transform.Find("Dialogue")?.GetComponent<TextMeshProUGUI>();
        dialogueBox = UI.transform.Find("DialogueBG").gameObject;
        interactIcon = UI.transform.Find("Interact").gameObject;
        interactText = UI.transform.Find("InteractText")?.GetComponent<TextMeshProUGUI>();
        skipText = UI.transform.Find("SkipText")?.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E) && !dialogueBox.activeSelf)
            {
                dialogueBox.SetActive(true);
                UI.GetComponent<UIManager>().ShowDialogue(dialogueText);
                interactIcon.SetActive(false);
                interactText.text = "";
                skipText.text = "Left Click to Skip";
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
        }
    }
}
