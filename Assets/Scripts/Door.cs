using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Door : MonoBehaviour
{
    private SpriteRenderer doorSprite;
    private BoxCollider doorCollider;
    private GameObject interact;
    public bool isOpen = false;
    private bool opening = false;
    private bool opened = false;
    public string LevelName;
    public int LevelEntryPoint;
    private AudioSource audioSource;
    public AudioClip openDoorSound;

    private GameObject UI;
    private TextMeshProUGUI dialogue;
    private GameObject dialogueBox;
    private GameObject interactIcon;
    private TextMeshProUGUI interactText;
    private TextMeshProUGUI skipText;
    private string dialogueText;
    // Start is called before the first frame update
    void Start()
    {
        //Get collider for room transition, the interact collider, and door sprite
        doorCollider = GetComponent<BoxCollider>();
        interact = transform.GetChild(0).gameObject;
        doorSprite = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = openDoorSound;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        UI = player.transform.Find("UI")?.gameObject;
        dialogue = UI.transform.Find("Dialogue")?.GetComponent<TextMeshProUGUI>();
        dialogueBox = UI.transform.Find("DialogueBG").gameObject;
        interactIcon = UI.transform.Find("Interact").gameObject;
        interactText = UI.transform.Find("InteractText")?.GetComponent<TextMeshProUGUI>();
        skipText = UI.transform.Find("SkipText")?.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Logic to make sure OpenDoor only runs once
        if (isOpen && !opening && !opened)
        {
            opening = true;
            StartCoroutine(OpenDoor());
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: When the player enter's the door's trigger, set the destination level/entry point ID in the manager, then load the level
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ExitLevel());
        }
    }

    IEnumerator OpenDoor()
    {
        //Enable collider for moving to next room
        doorCollider.enabled = true;
        float elapsedTime = 0f;
        
        //Hide dialogue/interact
        if (dialogueBox != null) {
            interactIcon.SetActive(false);
            interactText.text = "";
            if (dialogueBox.activeSelf == true) {
                dialogueBox.SetActive(false);
                dialogue.text = "";
                skipText.text = "";
            }
        }
        
        //Fade out doorSprite to reveal open door
        while (elapsedTime < 1.5f)
        {
            elapsedTime += Time.deltaTime;
            Color color = doorSprite.color;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / 2f);
            doorSprite.color = color;
            yield return null;
        }

        //Make sure doorSprite is fully transparent at the end
        Color transp = doorSprite.color;
        transp.a = 0f;
        doorSprite.color = transp;

        //Make interact prompt inactive
        interact.SetActive(false);
        
        opened = true;
        opening = false;
    }

    IEnumerator ExitLevel() {
        SceneFader fader = FindObjectOfType<SceneFader>();
        yield return StartCoroutine(fader.FadeOut());
        PersistantGameManager.SetTargetLevel(LevelName, LevelEntryPoint);
        SceneManager.LoadScene(LevelName);
    }
}
