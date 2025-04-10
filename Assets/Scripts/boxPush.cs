using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxPush : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip pushSound;
    private bool isBeingPushed = false;
    private Rigidbody rb;
    public float pushForce = 2.5f;
    private float minimumVelocityForSound = 0.1f; // Minimum velocity to play sound

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Make the sound 3D
        audioSource.volume = 1.0f; // Increased from 0.5f to 0.8f
    }

    void Update()
    {
        // Check if the box is moving
        if (rb.velocity.magnitude > minimumVelocityForSound)
        {
            if (!isBeingPushed && pushSound != null)
            {
                audioSource.clip = pushSound;
                audioSource.volume = 0.8f; // Set volume when playing
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
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 pushDirection = collision.contacts[0].point - transform.position;
            pushDirection = -pushDirection.normalized;
            pushDirection.y = 0; // Prevent vertical pushing
            rb.AddForce(pushDirection * pushForce, ForceMode.Force);
        }
    }
}
