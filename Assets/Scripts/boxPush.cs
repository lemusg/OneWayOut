using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxPush : MonoBehaviour
{
    public Rigidbody2D rb;
    public float pushForce = 2f;
    public bool isPushed = false;
    public bool isPushable = true;
    public PolygonCollider2D boundaryCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(isPushable && rb != null)
        {
            rb.gravityScale = 0f;
            rb.drag = 10f;  // Increase drag for more controlled movement
            rb.mass = 2f;  // Make it a bit heavier
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;  // Only freeze rotation
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPushable)
        {
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            
            // Calculate potential new position
            Vector2 potentialNewPosition = (Vector2)transform.position + (pushDirection * pushForce * Time.deltaTime);
            
            // Only apply force if new position would be within boundary
            if (boundaryCollider.OverlapPoint(potentialNewPosition))
            {
                rb.AddForce(pushDirection * pushForce, ForceMode2D.Force);
                isPushed = true;
            }
        }
        else
        {
            isPushed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }    
}
