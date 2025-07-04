using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public Transform planet; // Assign the planet in the Inspector
    public float gravity = 9.81f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        Vector3 gravityDirection = (planet.position - transform.position).normalized;
        rb.AddForce(gravityDirection * gravity, ForceMode.Acceleration);

        // Align the player "up" with the gravity direction
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }
}
