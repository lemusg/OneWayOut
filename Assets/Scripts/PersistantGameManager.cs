using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Idea: This script has static data/methods that will keep track of some data between scenes.
//Other scripts can look here, see what level was loaded and what "door" in the level we passed through, then move the player to that location.
public class PersistantGameManager : MonoBehaviour
{
    // Add singleton instance
    public static PersistantGameManager Instance { get; private set; }
    public static string LevelName = ""; //The level we are loading into
    public static int LevelEntryPoint = -1;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;

    void Awake()
    {
        // If there's an instance and it's not this one - destroy this one
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }
        
        // Make this the instance
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.volume = 0.25f; // Adjust this value to set default volume
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

    // Add method to control volume
    public void SetMusicVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play music when entering game scenes, but not the main menu or credits
        if (scene.name != "Main Menu" && scene.name != "Credits")
        {
            PlayMusic();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
    }
}