using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
     private Rigidbody2D rb;
    public float speed = 5f;
    public float maxDistance = 5f;
    [Range(0, 1)]
    [SerializeField] private float decelerationStart = 0.8f;
    [SerializeField] private float snapBackSpeed = 3f;
    [SerializeField] private float snapBackThreshold = 0.05f;

    [SerializeField] private InputActionReference move;
    private Vector2 moveDir;
    private Vector2 origin;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        origin = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = rb.position;
        float currentDistance = Vector2.Distance(currentPosition, origin);

        // Snap back to origin when no input
        if (moveDir.magnitude < 0.01f)
        {
            if (currentDistance > snapBackThreshold)
            {
                Vector2 directionToOrigin = (origin - currentPosition).normalized;
                rb.linearVelocity = directionToOrigin * (currentDistance * snapBackSpeed);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                rb.position = origin;
            }
        }
        // Handle movement input
        else
        {
            Vector2 desiredVelocity = moveDir * speed;
            float distanceFactor = Mathf.Clamp01(
                (currentDistance - decelerationStart * maxDistance) / 
                (maxDistance * (1 - decelerationStart))
            );

            // Apply boundary restrictions
            if (currentDistance > maxDistance)
            {
                Vector2 toOrigin = (origin - currentPosition).normalized;
                float outwardComponent = Vector2.Dot(desiredVelocity, -toOrigin);
                
                // Allow only inward movement
                if (outwardComponent > 0)
                {
                    desiredVelocity -= outwardComponent * -toOrigin;
                }
            }
            // Apply deceleration near boundaries
            else if (currentDistance > decelerationStart * maxDistance)
            {
                desiredVelocity *= Mathf.Clamp01(1 - distanceFactor);
            }

            rb.linearVelocity = desiredVelocity;
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        moveDir = move.action.ReadValue<Vector2>();
    }

    // Visualize boundaries in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, decelerationStart * maxDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, maxDistance);
    }
}
