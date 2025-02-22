using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float timer = 0f;
    private SpriteRenderer spriteRenderer;
    public Sprite sprite1;
    public Sprite sprite2;

    public float moveSpeed = 3f;
    
    private Rigidbody rb;
    private Vector3 isoForward = new Vector3(1f, 0f, 1f).normalized;
    private Vector3 isoRight = new Vector3(1f, 0f, -1f).normalized;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Convert input to isometric direction
        Vector3 direction = isoRight * x + isoForward * z;
        Vector3 velocity = direction.normalized * moveSpeed;
        rb.velocity = velocity;

        if (x!= 0)
        {
            spriteRenderer.flipX = x < 0;
        }

        Vector3 spritePosition = transform.position;
        spritePosition.y += .3f;
        transform.GetChild(0).position = spritePosition;

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            spriteRenderer.sprite = (spriteRenderer.sprite == sprite1) ? sprite2 : sprite1;
        }
    }
}