using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    // Start is called before the first frame update
    void Start()
    {
        //Get collider for room transition, the interact collider, and door sprite
        doorCollider = GetComponent<BoxCollider>();
        interact = transform.GetChild(0).gameObject;
        doorSprite = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = openDoorSound;
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
            PersistantGameManager.SetTargetLevel(LevelName, LevelEntryPoint);
            SceneManager.LoadScene(LevelName);
        }
    }

    IEnumerator OpenDoor()
    {
        //Enable collider for moving to next room
        doorCollider.enabled = true;
        float elapsedTime = 0f;
        
        //Fade out doorSprite to reveal open door
        while (elapsedTime < 2f)
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
}
