using UnityEngine;

public abstract class GravityField : MonoBehaviour
{
    public float gravityStrength = 9.81f;
    public float influenceRadius = 30f;

    public abstract Vector3 GetGravity(Vector3 position);

    public bool IsInRange(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= influenceRadius;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, influenceRadius);
    }
}
