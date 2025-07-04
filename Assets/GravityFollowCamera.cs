using UnityEngine;

public class GravityFollowCamera : MonoBehaviour
{
    public Transform target;               // Your Capsule
    public float distance = 6f;            // Camera distance
    public float height = 2f;              // Height above player
    public float followSpeed = 5f;         // Position lerp speed
    public float rotationSpeed = 5f;       // Rotation lerp speed

    void LateUpdate()
    {
        if (target == null) return;

        // Get gravity direction at the player
        Vector3 gravity = GravityManager.Instance.GetGravity(target.position);
        Vector3 up = -gravity.normalized;

        // Calculate desired camera position (behind and above)
        Vector3 forward = Vector3.ProjectOnPlane(target.forward, up).normalized;
        Vector3 desiredPosition = target.position - forward * distance + up * height;

        // Move camera smoothly
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Look at the player, using gravity-based up vector
        Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized, up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
