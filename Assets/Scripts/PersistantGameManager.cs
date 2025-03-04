using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Idea: This script has static data/methods that will keep track of some data between scenes.
//Other scripts can look here, see what level was loaded and what "door" in the level we passed through, then move the player to that location.
public class PersistantGameManager : MonoBehaviour
{
    public static string LevelName = ""; //The level we are loading into
    public static int LevelEntryPoint = -1;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Static method you can use to set the data when we pass through a door
    public static void SetTargetLevel(string level, int entrypoint)
    {
        //TODO: set this stuff
        LevelName = level;
        LevelEntryPoint = entrypoint;
    }
}