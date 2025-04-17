using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxPush2 : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip pushSound;
    private bool isBeingPushed = false;
    private Rigidbody rb;
    public float pushForce = 5.0f;
    private float minimumVelocityForSound = 0.1f;
    
    [Header("Linked Box")]
    public GameObject linkedBox; // The box that moves in opposite direction
    private Rigidbody linkedBoxRb; // Rigidbody of the linked box
    private bool isBeingMovedByLink = false; // Prevent infinite recursion
    private float linkedBoxSpeedMultiplier = 3.0f;  // Makes linked box move faster
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        // Configure Rigidbody for smoother movement
        rb.drag = 5f;  // Add some drag to prevent sliding
        rb.mass = 1f;  // Set a consistent mass
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1.0f;
        
        // Get the linked box's rigidbody if one is assigned
        if (linkedBox != null)
        {
            linkedBoxRb = linkedBox.GetComponent<Rigidbody>();
            if (linkedBoxRb == null)
            {
                linkedBoxRb = linkedBox.AddComponent<Rigidbody>();
            }
            // Apply same physics settings to linked box
            linkedBoxRb.drag = 5f;
            linkedBoxRb.mass = 1f;
            linkedBoxRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            Debug.Log($"Box {gameObject.name} linked to {linkedBox.name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the box is moving
        if (rb.velocity.magnitude > minimumVelocityForSound)
        {
            if (!isBeingPushed && pushSound != null)
            {
                audioSource.clip = pushSound;
                audioSource.volume = 0.8f;
                audioSource.Play();
                isBeingPushed = true;
            }
        }
        else
        {
            if (isBeingPushed)
            {
                audioSource.Stop();
                isBeingPushed = false;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isBeingMovedByLink)
        {
            // Calculate push direction for the primary box
            Vector3 pushDirection = collision.contacts[0].point - transform.position;
            pushDirection = -pushDirection.normalized;
            pushDirection.y = 0; // Prevent vertical pushing
            
            // Apply force to the primary box
            rb.velocity = pushDirection * pushForce;
            
            // Move the linked box in the opposite direction
            if (linkedBox != null && linkedBoxRb != null)
            {
                boxPush2 linkedScript = linkedBox.GetComponent<boxPush2>();
                if (linkedScript != null)
                {
                    linkedScript.isBeingMovedByLink = true;
                    // Apply increased speed to linked box
                    linkedBoxRb.velocity = -pushDirection * pushForce * linkedBoxSpeedMultiplier;
                    Debug.Log($"Moving linked box {linkedBox.name} in direction {-pushDirection} with speed {pushForce * linkedBoxSpeedMultiplier}");
                    linkedScript.isBeingMovedByLink = false;
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero;
            if (linkedBox != null && linkedBoxRb != null)
            {
                linkedBoxRb.velocity = Vector3.zero;
            }
        }
    }
}
