using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance { get; private set; }

    private GravityField[] fields;

    private GravityField lastField;

    void Awake()
    {
        Instance = this;
        fields = FindObjectsOfType<GravityField>();
    }

    public GravityField GetClosestField(Vector3 position)
    {
        GravityField bestField = null;
        float bestDist = float.MaxValue;

        foreach (var field in fields)
        {
            float dist = Vector3.Distance(position, field.transform.position);
            if (dist < bestDist && field.IsInRange(position))
            {
                bestDist = dist;
                bestField = field;
            }
        }

        return bestField;
    }

    public Vector3 GetGravity(Vector3 position)
    {
        GravityField closest = GetClosestField(position);
        return closest != null ? closest.GetGravity(position) : Vector3.zero;
    }

    public Vector3 GetGravityDirection(Vector3 position)
    {
        GravityField closest = GetClosestField(position);
        return closest != null ? (closest.transform.position - position).normalized : Vector3.zero;
    }

    public void NotifyIfFieldChanged(GameObject player)
    {
        GravityField current = GetClosestField(player.transform.position);
        if (current != lastField)
        {
            lastField = current;

            // Snap the player to new gravity orientation
            var controller = player.GetComponent<PlanetPlayerController>();
            if (controller != null)
            {
                Vector3 gravityDir = current.GetGravity(player.transform.position).normalized;
                controller.SnapToGravity(gravityDir);
            }
        }
    }
}