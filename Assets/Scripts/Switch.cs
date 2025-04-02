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
            Door d = door.GetComponent<Door>();
            d.isOpen = true;
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
            interactIcon.SetActive(false);
            interactText.text = "";
            isInteractable = false;
        }
    }

    public IEnumerator FlipSwitch()
    {
        GameObject switchObj = transform.GetChild(0).gameObject;
        Renderer switchRend = switchObj.GetComponent<Renderer>();
        switchRend.material = buttOn;
        yield return new WaitForSeconds(1f);
        switchRend.material = buttOff;
        int myIndex = transform.GetSiblingIndex();
        OnSwitchFlipped?.Invoke(myIndex);
    }
}
