using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject title;
    public Button button;
    public Sprite buttonImage1;
    public Sprite buttonImage2;
    public Button quit;
    public GameObject fade;
    private CanvasGroup canvasGroup;
    public GameObject popup;
    public GameObject quitConfirmation;
    
    [Header("Audio")]
    public AudioClip backgroundMusic;  // The music clip to play
    public AudioClip buttonClickSound; // Sound for button click
    private AudioSource audioSource;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;  // Volume control

    void Awake()
    {
        canvasGroup = fade.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeIn());
    }
    // Start is called before the first frame update
    void Start()
    {
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

    void Update()
    {
        if (popup.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Y)) {
                Application.Quit();
            } else if (Input.GetKeyDown(KeyCode.N)) {
                popup.SetActive(false);
                quitConfirmation.SetActive(false);
            }
        }
    }

    private void OnButtonClick()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        StartCoroutine(FadeOut());
    }

    private void Quit()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        popup.SetActive(true);
        quitConfirmation.SetActive(true);
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
        StartCoroutine(Animation());
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(2f);
        float elapsedTime = 0f;
        
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / 3f);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        fade.SetActive(false);        
        StartCoroutine(TitleAnimation());
    }

    public IEnumerator FadeOut()
    {
        fade.SetActive(true);
        float elapsedTime = 0f;
        
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / 3f);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
        SceneManager.LoadScene("RoomA");
    }
}
