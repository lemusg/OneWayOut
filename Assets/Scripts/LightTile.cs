using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightTile : MonoBehaviour
{
    public bool isInteractable = false;
    public bool isLit = false;
    public bool shouldBeLit = false;
    public static int correctTilesLit = 0;
    public static int incorrectTilesLit = 0;
    public int numTiles;
    
    public Material litMaterial;
    public Material unlitMaterial;
    public TextMeshProUGUI interactText;
    public GameObject interactIcon;
    public GameObject door;
    public GameObject popup;

    public AudioClip win;
    private AudioSource audioSource;

    void Start()
    {
        GetComponent<MeshRenderer>().material = unlitMaterial;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            if (popup != null)
                if (!popup.GetComponent<Popup>().triggered)
                    popup.GetComponent<Popup>().triggered = true;
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
        if (correctTilesLit == numTiles && incorrectTilesLit == 0)  
        {
            door.GetComponent<Door>().isOpen = true;
            if (win != null)
                audioSource.PlayOneShot(win);
            correctTilesLit = 0;
            incorrectTilesLit = 0;
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
