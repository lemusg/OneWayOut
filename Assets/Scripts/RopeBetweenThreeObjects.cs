using UnityEngine;

public class RopeBetweenThreeObjects : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3; 
    }

    void Update()
    {
        if (pointA && pointB && pointC)
        {
            lineRenderer.SetPosition(0, pointA.position);
            lineRenderer.SetPosition(1, pointB.position);
            lineRenderer.SetPosition(2, pointC.position);
        }
    }
}

