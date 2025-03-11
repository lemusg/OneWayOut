using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeedX = 0.1f; // Adjust speed
    public float scrollSpeedY = 0.0f;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
