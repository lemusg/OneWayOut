using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxPush : MonoBehaviour
{
    private Rigidbody rb;
    public float pushForce = 2.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
