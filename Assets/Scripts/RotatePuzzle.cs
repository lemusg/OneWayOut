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

    void Start()
    {
        correctRotations = 0;  // Reset the counter when scene starts
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
                        if (correctRotations >= 4)
                        {
                            door.SetActive(true);  // Or however you want to open the door
                        }
                    }
                }
                else if (isCorrect)
                {
                    isCorrect = false;
                    correctRotations--;
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
