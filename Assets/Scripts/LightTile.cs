using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightTile : MonoBehaviour
{
    public bool isInteractable = false;
    public bool isLit = false;
    public bool shouldBeLit = false;  // Designer sets this in inspector for correct solution
    private static int correctTilesLit = 0;
    private static int incorrectTilesLit = 0;
    
    public Material litMaterial;
    public Material unlitMaterial;
    public TextMeshProUGUI interactText;
    public GameObject interactIcon;
    public GameObject doorToUnlock;
    public GameObject doorSprite;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Setting initial material");
        if (unlitMaterial == null)
        {
            Debug.LogError("Unlit material is not assigned!");
        }
        GetComponent<MeshRenderer>().material = unlitMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            ToggleTile();
        }
    }

    void ToggleTile()
    {
        isLit = !isLit;
        GetComponent<MeshRenderer>().material = isLit ? litMaterial : unlitMaterial;
        
        if (shouldBeLit)
        {
            correctTilesLit += isLit ? 1 : -1;
        }
        else
        {
            incorrectTilesLit += isLit ? 1 : -1;
        }

        CheckPuzzleCompletion();
    }

    void CheckPuzzleCompletion()
    {
        // Adjust these values based on your puzzle design
        if (correctTilesLit == 3 && incorrectTilesLit == 0)  
        {
            doorToUnlock.SetActive(true);
            doorSprite.SetActive(true);
        }
        else
        {
            doorToUnlock.SetActive(false);
            doorSprite.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactIcon.SetActive(true);
            interactText.text = "Interact";
            isInteractable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactIcon.SetActive(false);
            interactText.text = "";
            isInteractable = false;
        }
    }
}
