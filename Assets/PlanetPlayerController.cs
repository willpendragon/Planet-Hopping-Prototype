using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetPlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float escapeForce = 20f;
    //public Transform planet;

    private Rigidbody rb;
    private bool isGrounded;
    private bool jumpRequested;
    private bool escapeRequested;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            escapeRequested = true;
        }
    }

    void FixedUpdate()
    {
        GravityManager.Instance.NotifyIfFieldChanged(gameObject);

        // Step 1: Gravity
        Vector3 gravity = GravityManager.Instance.GetGravity(transform.position);
        Vector3 gravityDirection = gravity.normalized;

        rb.AddForce(gravity, ForceMode.Acceleration);

        // Step 2: Align to gravity
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));

        // Step 3: Movement input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 playerUp = -gravityDirection;
        Vector3 playerRight = Vector3.Cross(playerUp, transform.forward).normalized;
        Vector3 playerForward = Vector3.Cross(playerRight, playerUp).normalized;
        Vector3 moveDir = (playerForward * v + playerRight * h).normalized;

        Vector3 desiredVelocity = moveDir * moveSpeed;
        Vector3 gravityVelocity = Vector3.Project(rb.velocity, gravityDirection);
        rb.velocity = desiredVelocity + gravityVelocity;

        // Step 4: Jump
        if (jumpRequested)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
            isGrounded = false;
        }

        // Step 5: Escape Launch
        if (escapeRequested)
        {
            rb.AddForce(transform.up * escapeForce, ForceMode.Impulse);
            escapeRequested = false;
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 gravityDir = GravityManager.Instance.GetGravity(transform.position).normalized;
            SnapToGravity(gravityDir);
            Debug.Log("Manual snap triggered");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    public void SnapToGravity(Vector3 gravityDirection)
    {
        Vector3 up = -gravityDirection;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;

        if (forward == Vector3.zero)
            forward = Vector3.forward;

        Quaternion snappedRotation = Quaternion.LookRotation(forward, up);
        transform.rotation = snappedRotation;
    }

}
