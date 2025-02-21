using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PolygonCollider2D boundaryCollider;
    public float moveSpeed = 3f;

    private Vector3 moveDirection;

    public Sprite sprite1;
    public Sprite sprite2;
    private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    private float switchTime = 0.5f;

    public GameObject gameUI;

    public bool canMove = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!canMove) {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= switchTime)
        {
            spriteRenderer.sprite = spriteRenderer.sprite == sprite1 ? sprite2 : sprite1;
            timer = 0f;
        }

        if (gameUI.activeSelf) {
            moveDirection = Vector3.zero;
            // Store the current position in case we need to revert
            Vector3 originalPosition = transform.position;
            Vector3 potentialNewPosition = originalPosition;

            // Try moving horizontally
            if (Input.GetKey(KeyCode.A))
            {
                potentialNewPosition += Vector3.left * moveSpeed * Time.deltaTime;
                spriteRenderer.flipX = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                potentialNewPosition += Vector3.right * moveSpeed * Time.deltaTime;
                spriteRenderer.flipX = false;
            }

            // Apply horizontal movement if valid
            if (boundaryCollider.OverlapPoint(potentialNewPosition))
            {
                transform.position = potentialNewPosition;
            }
            
            // Reset to current position for vertical movement
            potentialNewPosition = transform.position;

            // Try moving vertically
            if (Input.GetKey(KeyCode.W))
            {
                potentialNewPosition += Vector3.up * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                potentialNewPosition += Vector3.down * moveSpeed * Time.deltaTime;
            }

            // Apply vertical movement if valid
            if (boundaryCollider.OverlapPoint(potentialNewPosition))
            {
                transform.position = potentialNewPosition;
            }
        }
    }
}