using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Switch : MonoBehaviour
{
    public bool isInteractable = false;
    public bool flipped = false;
    public bool correct = false;
    private static int correctSwitchesFlipped = 0;
    private static int incorrectSwitchesFlipped = 0;
    public Material flippedMaterial;
    public Material originalMaterial;
    public TextMeshProUGUI interactText;
    public GameObject interactIcon;
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = originalMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                flipped = !flipped;
                transform.GetChild(0).GetComponent<MeshRenderer>().material = flipped ? flippedMaterial : originalMaterial;
                if (correct)
                {
                    correctSwitchesFlipped += flipped ? 1 : -1;
                }
                else
                {
                    incorrectSwitchesFlipped += flipped ? 1 : -1;
                }
                if (correctSwitchesFlipped == 2 && incorrectSwitchesFlipped == 0)
                {
                    door.SetActive(true);
                }
                Debug.Log("Correct Switches Flipped: " + correctSwitchesFlipped);
                Debug.Log("Incorrect Switches Flipped: " + incorrectSwitchesFlipped);
                Debug.Log("Door Active: " + door.activeSelf);
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
            interactIcon.SetActive(false);
            interactText.text = "";
            isInteractable = false;
        }
    }
}
