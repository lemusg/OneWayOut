using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Credits : MonoBehaviour
{
    public GameObject fadeIn;
    public GameObject fadeOut;
    public bool fadeDone;
    public Button quit;
    public GameObject UI;
    public TextMeshProUGUI credits;
    public GameObject image;
    private bool isQuit;
    
    [Header("Audio")]
    public AudioClip backgroundMusic;  // The music clip to play
    public AudioClip buttonClickSound; // Sound for button click
    public AudioClip imageSlam;
    private AudioSource audioSource;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;  // Volume control

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        quit.interactable = false;
        quit.onClick.AddListener(Quit);

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
            quit.gameObject.SetActive(true);
            quit.interactable = true;
        }
    }

    private void Quit()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        isQuit = true;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(6f);
        fadeIn.SetActive(false);
        fadeDone = true;
        StartCoroutine(ScrollCredits());
    }

    private IEnumerator FadeOut()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(6f);
        if (isQuit)
            Application.Quit();
    }

    private IEnumerator ScrollCredits()
    {
        credits.gameObject.SetActive(true);
        float elapsedTime = 0f;
        
        RectTransform rectTransform = credits.GetComponent<RectTransform>();
        Vector2 startPos = new Vector2(rectTransform.anchoredPosition.x, -1900f);
        Vector2 endPos = new Vector2(rectTransform.anchoredPosition.x, 1300f);

        rectTransform.anchoredPosition = startPos;

        // Scroll the credits over time
        while (elapsedTime < 15f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / 15f);
            Vector2 newPos = Vector2.Lerp(startPos, endPos, t);
            rectTransform.anchoredPosition = newPos;

            yield return null;
        }

        rectTransform.anchoredPosition = endPos;
        
        yield return new WaitForSeconds(3f);

        CanvasGroup canvasGroup = credits.GetComponent<CanvasGroup>();
        
        float fadeElapsedTime = 0f;
        while (fadeElapsedTime < 2f)
        {
            fadeElapsedTime += Time.deltaTime;
            float fadeT = Mathf.Clamp01(fadeElapsedTime / 2f);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeT);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        float audioElapsedTime = 0f;
        while (audioElapsedTime < 1f)
        {
            audioElapsedTime += Time.deltaTime;
            float audioT = Mathf.Clamp01(audioElapsedTime / 1f);
            audioSource.volume = Mathf.Lerp(0.5f, 0f, audioT);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0.5f;
        
        yield return new WaitForSeconds(2f);

        image.SetActive(true);

        CanvasGroup imageCanvasGroup = image.GetComponent<CanvasGroup>();

        audioSource.PlayOneShot(imageSlam);

        yield return new WaitForSeconds(3f);

        StartCoroutine(FadeOut());

        SceneManager.LoadScene("Main Menu");
    }
}
