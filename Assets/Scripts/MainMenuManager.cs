using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public TextMeshProUGUI title;
    // Whether the title is currently growing or shrinking
    public bool size;
    public GameObject fadeIn;
    public GameObject fadeOut;
    public bool fadeDone;
    public Button button;
    public Sprite buttonImage1;
    public Sprite buttonImage2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        button.interactable = false;
        StartCoroutine(Animation());
        button.onClick.AddListener(FadeOut);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeDone) {
            button.interactable = true;
        }
    }

    private void FadeOut() {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator Animation() {
        while (true) {
            if (size) {
                title.fontSize += 2;
                button.transform.localScale += new Vector3(1f, 1f, 1f);
                button.image.sprite = buttonImage2;
            } else {
                title.fontSize -= 2;
                button.transform.localScale -= new Vector3(1f, 1f, 1f);
                button.image.sprite = buttonImage1;
            }
            size = !size;
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator FadeIn() {
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(6f);
        fadeIn.SetActive(false);
        fadeDone = true;
    }

    private IEnumerator FadeOutRoutine() {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("Game");
    }
}
