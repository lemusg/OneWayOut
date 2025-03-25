using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public TextMeshProUGUI dialogue;
    public GameObject dialogueBox;
    public UIManager uiManager;
    public GameObject interactIcon;
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI skipText;
    public string dialogueText;
    private bool isInteractable = false;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E) && !dialogueBox.activeSelf)
            {
                dialogueBox.SetActive(true);
                uiManager.ShowDialogue(dialogueText);
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
            uiManager.SkipTyping();
            dialogue.text = "";
            interactText.text = "";
            skipText.text = "";
            isInteractable = false;
        }
    }
}
