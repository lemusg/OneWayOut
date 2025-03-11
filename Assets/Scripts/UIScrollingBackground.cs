using UnityEngine;
using UnityEngine.UI;

public class UIScrollingBackground : MonoBehaviour
{
    public float scrollSpeedX = 0.01f;
    public float scrollSpeedY = 0.01f;
    private RawImage image;
    private float offsetX, offsetY;

    void Start()
    {
        image = GetComponent<RawImage>();
    }

    void Update()
    {
        offsetX += Time.deltaTime * scrollSpeedX;
        offsetY += Time.deltaTime * scrollSpeedY;
        
        image.uvRect = new Rect(offsetX, offsetY, 1, 1);
    }
}