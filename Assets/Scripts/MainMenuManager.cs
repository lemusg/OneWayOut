using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject title;
    public GameObject fadeIn;
    public GameObject fadeOut;
    public bool fadeDone;
    public Button button;
    public Sprite buttonImage1;
    public Sprite buttonImage2;
    public Button quit;
    
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
        quit.interactable = false;
        StartCoroutine(Animation());
        button.onClick.AddListener(OnButtonClick);
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
            button.interactable = true;
            quit.interactable = true;
        }
    }

    private void OnButtonClick()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        FadeOut(false);
    }

    private void Quit()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        FadeOut(true);
    }

    private void FadeOut(bool isQuit)
    {
        StartCoroutine(FadeOutRoutine(isQuit));
    }

    private IEnumerator Animation()
    {
        while (true)
        {
            button.image.sprite = buttonImage2;
            button.transform.localScale = new Vector3(4f, 4f, 4f);
            yield return new WaitForSeconds(1f);
            button.image.sprite = buttonImage1;
            button.transform.localScale = new Vector3(3f, 3f, 3f);
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator TitleAnimation()
    {
        Vector3 startPos = title.transform.position + Vector3.up * 1000f;
        Vector3 targetPos = title.transform.position;
        title.transform.position = startPos;
        title.SetActive(true);
        float duration = 4f;
        float elapsed = 0f;
        float shakeAmount = 15f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
            
            if (t < 3f)
            {
                currentPos += new Vector3(
                    Random.Range(-shakeAmount, shakeAmount),
                    Random.Range(-shakeAmount, shakeAmount),
                    0
                ) * (1 - t);
            }
            
            title.transform.position = currentPos;
            yield return null;
        }
        title.transform.position = targetPos;
        button.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
    }

    private IEnumerator FadeIn()
    {
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(6f);
        fadeIn.SetActive(false);
        StartCoroutine(TitleAnimation());
        fadeDone = true;
    }

    private IEnumerator FadeOutRoutine(bool isQuit)
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(6f);
        if (isQuit)
        {
            Application.Quit();
        } else {
            SceneManager.LoadScene("Scenes/LevelOne/RoomA");
        }
    }
}
