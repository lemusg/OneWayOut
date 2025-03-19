using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float animationTimer = 0f;
    private int currentFrame = 0;
    private SpriteRenderer spriteRenderer;
    
    // Separate arrays for different animation states
    public Sprite[] idleSprites;    // Array for idle animation frames
    public Sprite[] walkingSprites; // Array for walking animation frames
    
    public float animationFrameRate = 0.5f;
    private bool isMoving = false;

    public float moveSpeed = 3f;
    
    private Rigidbody rb;
    private Vector3 isoForward = new Vector3(1f, 0f, 1f).normalized;
    private Vector3 isoRight = new Vector3(1f, 0f, -1f).normalized;
    
    private AudioSource audioSource;
    public AudioClip footstepSound;
    private float footstepTimer = 0f;
    private float footstepInterval = 0.5f; // Adjust this value to change how often the footstep plays

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 direction = isoRight * x + isoForward * z;
        Vector3 velocity = direction.normalized * moveSpeed;
        rb.velocity = velocity;

        // Determine if character is moving
        isMoving = direction.magnitude > 0.1f;

        // Handle sprite flipping instead of using separate left/right sprites
        if (x != 0)
        {
            spriteRenderer.flipX = x < 0;
        }

        // Animation state machine
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationFrameRate)
        {
            animationTimer = 0f;
            
            if (isMoving)
            {
                // Play walking animation
                currentFrame = (currentFrame + 1) % walkingSprites.Length;
                spriteRenderer.sprite = walkingSprites[currentFrame];
            }
            else
            {
                // Play idle animation
                currentFrame = (currentFrame + 1) % idleSprites.Length;
                spriteRenderer.sprite = idleSprites[currentFrame];
            }
        }

        // Adjust these values as needed
        Vector3 spritePosition = transform.position;
        transform.GetChild(0).position = spritePosition;

        // Add this footstep sound logic
        if (direction.magnitude > 0.1f) // If the player is moving
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                footstepTimer = 0f;
                if (footstepSound != null)
                {
                    audioSource.PlayOneShot(footstepSound);
                }
            }
        }
        else
        {
            footstepTimer = footstepInterval; // Reset timer when not moving
        }
    }
}