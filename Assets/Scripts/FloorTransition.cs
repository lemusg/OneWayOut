using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FloorTransition : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake() {
        canvasGroup = GameObject.Find("Fade").GetComponent<CanvasGroup>();
    }

    void Start()
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(Transition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Transition() {
        yield return (FadeIn());
        yield return (ScrollText());
        yield return (FadeOut());
        SceneManager.LoadScene("RoomA2");
    }

    private IEnumerator FadeIn() {
        float elapsedTime = 0f;
        
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / 3f);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
    }

    private IEnumerator FadeOut()
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

    private IEnumerator ScrollText() {
        TextMeshProUGUI levelText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        levelText.text = "";
        foreach (char c in "Level 2")
        {
            levelText.text += c;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
