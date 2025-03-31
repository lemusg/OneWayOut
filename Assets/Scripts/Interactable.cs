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
    private bool isFloating = false;
    private bool shouldStartFloating = false;
    private Vector3 targetPosition;
    private float floatSpeed = 1.5f;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private UIManager uiManager;
    private SpriteRenderer spriteRenderer;
    
    [Header("Paper Float Settings")]
    [SerializeField] private float screenOffsetRight = 50f;
    [SerializeField] private float screenOffsetTop = 50f;
    [SerializeField] private float finalScale = 0.4f; // The paper will scale to 40% of its original size
    
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
        initialScale = transform.localScale;
        uiManager = UI.GetComponent<UIManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable && !isFloating)
        {
            if (Input.GetKeyDown(KeyCode.E) && !dialogueBox.activeSelf)
            {
                dialogueBox.SetActive(true);
                UI.GetComponent<UIManager>().ShowDialogue(dialogueText);
                interactIcon.SetActive(false);
                interactText.text = "";
                skipText.text = "Left Click to Skip";
                
                if (isPaper)
                {
                    shouldStartFloating = true;
                }
            }
        }

        // Check for mouse click to close dialogue and start floating
        if (shouldStartFloating && Input.GetMouseButtonDown(0))
        {
            dialogueBox.SetActive(false);
            UI.GetComponent<UIManager>().SkipTyping();
            dialogue.text = "";
            skipText.text = "";
            StartFloatingAnimation();
            shouldStartFloating = false;
            
            // Add the paper to clues when collected
            if (isPaper && spriteRenderer != null)
            {
                Debug.Log("Adding clue");
                uiManager.AddClue(spriteRenderer.sprite, dialogueText);
            }
        }
        
        if (isFloating)
        {
            // Move the paper towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * floatSpeed);
            
            // Scale down the paper to the target scale
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * floatSpeed);
            
            // If paper is close enough to target position, destroy it
            if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void StartFloatingAnimation()
    {
        isFloating = true;
        
        // Calculate target position in top-right corner where the question mark is
        Vector3 screenPos = new Vector3(Screen.width - screenOffsetRight, Screen.height - screenOffsetTop, 10f);
        targetPosition = Camera.main.ScreenToWorldPoint(screenPos);
        
        // Set the target scale to be a percentage of the original scale
        targetScale = initialScale * finalScale;
        
        // Disable colliders
        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false;
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
            shouldStartFloating = false;  // Reset floating flag if player walks away
        }
    }
}
