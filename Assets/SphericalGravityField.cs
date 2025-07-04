using UnityEngine;

public class SphericalGravityField : GravityField
{
    public override Vector3 GetGravity(Vector3 position)
    {
        Vector3 dir = (transform.position - position).normalized;
        return dir * gravityStrength;
    }
}
