using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SimonSaysManager : MonoBehaviour
{
    public List<Switch> correctSequence = new List<Switch>();
    public float startDelay = 2f; // Delay before showing sequence

    void Start()
    {
        // Wait a bit to ensure all switches are initialized
        StartCoroutine(InitializeSequence());
    }

    IEnumerator InitializeSequence()
    {
        yield return new WaitForSeconds(startDelay);
        
        // Now we can safely set up the sequence
        SetupSequence();
        StartCoroutine(ShowSequence());
    }

    void SetupSequence()
    {
        correctSequence.Clear();
        // Add switches in the desired order
        // You can drag and drop the switches in the Unity Inspector
        // or find them by other means
    }

    IEnumerator ShowSequence()
    {
        Switch.isShowingSequence = true;
        Switch.canPlayerInput = false;
        
        foreach (Switch switchObj in correctSequence)
        {
            switchObj.transform.GetChild(0).GetComponent<MeshRenderer>().material = switchObj.flippedMaterial;
            yield return new WaitForSeconds(switchObj.sequenceDelay);
            switchObj.transform.GetChild(0).GetComponent<MeshRenderer>().material = switchObj.originalMaterial;
            yield return new WaitForSeconds(switchObj.sequenceDelay / 2);
        }

        Switch.isShowingSequence = false;
        Switch.canPlayerInput = true;
    }

    public void ResetAndShowSequence()
    {
        StartCoroutine(ShowSequence());
    }
}