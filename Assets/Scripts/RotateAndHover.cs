using UnityEngine;

public class RotateAndHover : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation
    public float hoverHeight = 0.5f; // Height above ground to hover
    public float hoverSpeed = 1f; // Speed of hovering

    private float initialY; // Initial Y position of the object

    void Start()
    {
        initialY = transform.position.y;
    }

    void Update()
    {
        // Rotate the object around its up axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Hover the object up and down
        float newY = initialY + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
