using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RotatePuzzle : MonoBehaviour
{
    public int correctRotation;
    public GameObject door;
    public bool isInteractable = false;
    public TextMeshProUGUI interactText;
    public GameObject interactIcon;
    private static int correctRotations = 0;
    private bool isCorrect = false;
    public Material correctMat;
    public Material incorrectMat;
    private MeshRenderer objectRenderer;

    void Start()
    {
        correctRotations = 0;  // Reset the counter when scene starts
        objectRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.Rotate(0, 45, 0);
                if ((int) transform.rotation.eulerAngles.y == correctRotation)
                {
                    if (!isCorrect)
                    {
                        isCorrect = true;
                        correctRotations++;
                        GameObject beam = transform.GetChild(1).gameObject;
                        beam.SetActive(true);
                        if (correctRotations >= 4)
                        {
                            door.GetComponent<Door>().isOpen = true;
                        }
                    }
                }
                else if (isCorrect)
                {
                    isCorrect = false;
                    correctRotations--;
                    GameObject beam = transform.GetChild(1).gameObject;
                    beam.SetActive(false);
                }
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
