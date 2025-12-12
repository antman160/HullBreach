using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public Rigidbody2D rb;
    public ShipAttributes attributes;

    public float engineFactor = 1f; // modified by engine subsystems

    [Header("Bounds")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attributes = GetComponent<ShipAttributes>();
    }

    private void FixedUpdate()
    {
        ClampSpeed();
    }

    private void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude > attributes.maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * attributes.maxSpeed;
        }
    }

    // ---------------------------------------------------------
    // Thrust Functions (Rear-engine, Reverse, Strafe)
    // ---------------------------------------------------------

    public void ThrustForward()
    {
        Vector2 forward = transform.up;
        rb.AddForce(forward * attributes.maxThrust * engineFactor, ForceMode2D.Force);
    }

    public void ThrustReverse()
    {
        Vector2 backward = -transform.up;
        rb.AddForce(backward * attributes.maxThrust * 0.25f * engineFactor, ForceMode2D.Force);
    }

    public void ThrustStrafe(float direction)
    {
        // direction = +1 right, -1 left
        Vector2 right = new Vector2(transform.up.y, -transform.up.x);
        rb.AddForce(right * direction * attributes.maxThrust * 0.2f * engineFactor, ForceMode2D.Force);
    }

    // ---------------------------------------------------------
    // Rotation
    // ---------------------------------------------------------

    public void RotateTowards(Vector3 target, float turnTorque)
    {
        Vector2 dir = ((Vector2)target - (Vector2)transform.position).normalized;
        float desiredAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.z, desiredAngle);

        rb.AddTorque(-angleDiff * turnTorque * Time.fixedDeltaTime);
    }
}
