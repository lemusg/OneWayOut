using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private GameObject popup;
    public GameObject door;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Transform popupTransform = transform.parent.Find("Popup");
        if (popupTransform != null) {
            popup = popupTransform.gameObject;
        }
    }

    void Start()
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / 3f);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;

        if (popup != null) {
            popup.SetActive(true);
        }
        if (door != null) {
            door.GetComponent<Door>().isOpen = true;
        }
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / 3f);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
}