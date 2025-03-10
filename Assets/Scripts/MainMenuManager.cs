using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Image titleImage;
    public GameObject fadeIn;
    public GameObject fadeOut;
    public bool fadeDone;
    public Button button;
    public Sprite buttonImage1;
    public Sprite buttonImage2;
    
    [Header("Audio")]
    public AudioClip backgroundMusic;  // The music clip to play
    public AudioClip buttonClickSound; // Sound for button click
    private AudioSource audioSource;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;  // Volume control

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        button.interactable = false;
        StartCoroutine(Animation());
        button.onClick.AddListener(OnButtonClick);

        // Setup and play background music
        audioSource = gameObject.AddComponent<AudioSource>();
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.volume = musicVolume;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeDone)
        {
            button.interactable = true;
        }
    }

    private void OnButtonClick()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        FadeOut();
    }

    private void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator Animation()
    {
        while (true)
        {
            button.image.sprite = buttonImage2;
            yield return new WaitForSeconds(1f);
            button.image.sprite = buttonImage1;
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator FadeIn()
    {
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(6f);
        fadeIn.SetActive(false);
        fadeDone = true;
    }

    private IEnumerator FadeOutRoutine()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("Scenes/LevelOne/RoomA");
    }
}
