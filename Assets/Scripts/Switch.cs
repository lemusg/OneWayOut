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

    // New variables for Simon Says
    public static bool isShowingSequence = false;
    public static bool canPlayerInput = false;
    public static List<Switch> allSwitches = new List<Switch>();
    public static List<Switch> correctSequence = new List<Switch>();
    public float sequenceDelay = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = originalMaterial;
        allSwitches.Add(this);
        
        // Wait a frame to ensure all switches are added
        StartCoroutine(InitializeSequence());
    }

    IEnumerator InitializeSequence()
    {
        // Wait for next frame to ensure all switches are added
        yield return new WaitForEndOfFrame();
        
        // Only the last switch should start the sequence
        if (transform == allSwitches[allSwitches.Count - 1].transform)
        {
            StartCoroutine(ShowSequence());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow interaction when not showing sequence and player input is enabled
        if (isInteractable && !isShowingSequence && canPlayerInput)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                FlipSwitch();
            }
        }
    }

    void FlipSwitch()
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
            // Wrong switch flipped - reset the puzzle
            StartCoroutine(ResetPuzzle());
        }

        if (correctSwitchesFlipped == 2 && incorrectSwitchesFlipped == 0)
        {
            door.GetComponent<Door>().isOpen = true;
        }
    }

    IEnumerator ShowSequence()
    {
        isShowingSequence = true;
        canPlayerInput = false;
        correctSequence.Clear();
        
        // Now we can safely add switches since they're all initialized
        // Add your desired sequence here (adjust indices as needed)
        if (allSwitches.Count >= 2)  // Safety check
        {
            correctSequence.Add(allSwitches[0]);
            correctSequence.Add(allSwitches[1]);
        }

        // Show the sequence
        foreach (Switch switchObj in correctSequence)
        {
            switchObj.transform.GetChild(0).GetComponent<MeshRenderer>().material = flippedMaterial;
            yield return new WaitForSeconds(sequenceDelay);
            switchObj.transform.GetChild(0).GetComponent<MeshRenderer>().material = originalMaterial;
            yield return new WaitForSeconds(sequenceDelay / 2);
        }

        isShowingSequence = false;
        canPlayerInput = true;
    }

    IEnumerator ResetPuzzle()
    {
        canPlayerInput = false;
        yield return new WaitForSeconds(1f);
        
        // Reset all switches
        foreach (Switch switchObj in allSwitches)
        {
            switchObj.flipped = false;
            switchObj.transform.GetChild(0).GetComponent<MeshRenderer>().material = originalMaterial;
        }
        correctSwitchesFlipped = 0;
        incorrectSwitchesFlipped = 0;
        
        // Show sequence again using the manager
        FindObjectOfType<SimonSaysManager>().ResetAndShowSequence();
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
