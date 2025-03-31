using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private List<Transform> lights;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            Transform gChild = child.GetChild(0);
            Light flame = gChild.GetComponent<Light>();
            StartCoroutine(ChangeColor(flame));
        } 
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform light in lights) {
            Light flame = light.GetComponent<Light>();
            Color newColor = new Color(1.0f, Random.Range(0.2f, 1.0f), Random.Range(0.0f, 0.2f));
            flame.color = newColor;
        }
    }

    IEnumerator ChangeColor(Light light) {
        while (true) {
            Color newColor = new Color(1.0f, Random.Range(0.2f, 1.0f), Random.Range(0.0f, 0.2f));
            Color initialColor = light.color;
            float t = 0f;

            while (t < 1f) {
                t += Time.deltaTime * 2f;
                light.color = Color.Lerp(initialColor, newColor, t);
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }
}
