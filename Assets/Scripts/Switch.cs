using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Switch : MonoBehaviour
{
    private bool isInteractable = false;
    private GameObject UI;
    private GameObject interactIcon;
    private TextMeshProUGUI interactText;
    public GameObject door;
    public delegate void SwitchFlipped(int index);
    public static event SwitchFlipped OnSwitchFlipped;
    public static int correctSwitches;
    public Material buttOn;
    public Material buttOff;
    public static bool canFlip = false;
    public static bool win = false;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        UI = player.transform.Find("UI")?.gameObject;
        interactIcon = UI.transform.Find("Interact").gameObject;
        interactText = UI.transform.Find("InteractText")?.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(FlipSwitch());
            }
        }

        if (correctSwitches == 5) {
            win = true;
            interactIcon.SetActive(false);
            interactText.text = "";
            foreach (Transform child in transform.parent) {
                isInteractable = false;
                GameObject switchObj = child.GetChild(0).gameObject;
                Renderer switchRend = switchObj.GetComponent<Renderer>();
                switchRend.material = buttOn;
            }
            Door d = door.GetComponent<Door>();
            d.isOpen = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canFlip)
        {
            interactIcon.SetActive(true);
            interactText.text = "Interact";
            isInteractable = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!interactIcon.activeSelf && canFlip) {
                interactIcon.SetActive(true);
                interactText.text = "Interact";
                isInteractable = true;
            } else if (interactIcon.activeSelf && !canFlip) {
                interactIcon.SetActive(false);
                interactText.text = "";
                isInteractable = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactIcon.SetActive(false);
            interactText.text = "";
            isInteractable = false;
        }
    }

    public IEnumerator FlipSwitch()
    {
        canFlip = false;
        interactIcon.SetActive(false);
        interactText.text = "";
        foreach (Transform child in transform.parent) {
            child.GetComponent<Switch>().isInteractable = false;
        }
        GameObject switchObj = transform.GetChild(0).gameObject;
        Renderer switchRend = switchObj.GetComponent<Renderer>();
        switchRend.material = buttOn;
        yield return new WaitForSeconds(1f);
        switchRend.material = buttOff;
        int myIndex = transform.GetSiblingIndex();
        OnSwitchFlipped?.Invoke(myIndex);
    }
}
