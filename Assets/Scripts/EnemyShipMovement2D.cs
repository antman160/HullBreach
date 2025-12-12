using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(ShipAttributes))]
public class EnemyShipMovement2D : MonoBehaviour
{
    [Header("Target")]
    public Transform playerTarget;

    [Header("Range Control")]
    public float desiredRange = 25f;
    public float rangeDeadZone = 5f;

    [Header("Movement Tuning")]
    public float forwardThrustFactor = 1.0f;
    public float reverseThrustFactor = 0.4f;
    public float strafeThrustFactor = 0.5f;
    public float maxTurnSpeed = 80f;

    [Header("World Bounds (Same As Player)")]
    public Vector2 worldMin = new Vector2(-40f, -40f);
    public Vector2 worldMax = new Vector2(40f, 40f);

    [Header("Edge Avoidance")]
    public float avoidDistance = 5f;
    public float avoidTurnBoost = 1.5f;
    public float edgePushStrength = 0.8f;

    private Rigidbody2D rb;
    private ShipAttributes attributes;

    private EngineSubsystem[] engines;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attributes = GetComponent<ShipAttributes>();

        rb.gravityScale = 0f;
        rb.mass = attributes.mass;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        // find all engines on this enemy ship
        engines = GetComponentsInChildren<EngineSubsystem>();
    }

    private void FixedUpdate()
    {
        if (playerTarget == null)
            return;

        HandleMovement();
        ClampSpeed();
    }

    private void LateUpdate()
    {
        ClampToWorldBounds();
    }

    // ----------------------------------------------------
    //  ENGINE SUBSYSTEM FACTOR
    // ----------------------------------------------------
    private float GetEngineFactor()
    {
        if (engines == null || engines.Length == 0)
            return 1f;

        int destroyed = 0;
        foreach (var e in engines)
            if (!e.IsOnline)
                destroyed++;

        float half = engines.Length * 0.5f;

        if (destroyed == 0)
            return 1f;       // all engines online
        if (destroyed <= half)
            return 0.6f;     // at or below half  60%
        return 0.25f;        // all destroyed  limp mode
    }

    private void HandleMovement()
    {
        Vector2 pos = rb.position;
        Vector2 toPlayer = (Vector2)(playerTarget.position - transform.position);
        float dist = toPlayer.magnitude;
        Vector2 dirToPlayer = toPlayer.normalized;

        // -------------------------
        // EDGE AVOIDANCE
        // -------------------------
        bool avoidingEdge = false;
        Vector2 avoidDir = Vector2.zero;

        if (pos.x < worldMin.x + avoidDistance) { avoidingEdge = true; avoidDir += Vector2.right; }
        if (pos.x > worldMax.x - avoidDistance) { avoidingEdge = true; avoidDir += Vector2.left; }
        if (pos.y < worldMin.y + avoidDistance) { avoidingEdge = true; avoidDir += Vector2.up; }
        if (pos.y > worldMax.y - avoidDistance) { avoidingEdge = true; avoidDir += Vector2.down; }

        if (avoidingEdge && avoidDir != Vector2.zero)
            avoidDir = avoidDir.normalized;

        Vector2 velocity = rb.linearVelocity;
        bool lowSpeed = velocity.sqrMagnitude < 0.1f;

        Vector2 desiredDir;

        if (avoidingEdge)
            desiredDir = avoidDir;
        else if (!lowSpeed)
            desiredDir = velocity.normalized;
        else
            desiredDir = dirToPlayer;

        float desiredAngle = Mathf.Atan2(desiredDir.y, desiredDir.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = rb.rotation;

        float turnBoost = avoidingEdge ? avoidTurnBoost : 1f;
        float rotateSpeed = maxTurnSpeed * turnBoost;

        float finalAngle = Mathf.MoveTowardsAngle(currentAngle, desiredAngle, rotateSpeed * Time.deltaTime);
        rb.MoveRotation(finalAngle);

        // -------------------------
        // THRUST INPUT
        // -------------------------
        float thrustInput = 0f;

        if (avoidingEdge)
        {
            thrustInput = edgePushStrength;
        }
        else
        {
            if (dist > desiredRange + rangeDeadZone)
                thrustInput = 1f;
            else if (dist < desiredRange - rangeDeadZone)
                thrustInput = -reverseThrustFactor;
        }

        // orbit strafe
        float strafeInput = 0f;
        if (!avoidingEdge)
        {
            float side = Mathf.Sign(Vector3.Cross(transform.up, toPlayer).z);
            strafeInput = side * strafeThrustFactor;
        }

        // ----------------------------------------------------
        //  APPLY FORCES WITH ENGINE MULTIPLIER
        // ----------------------------------------------------
        float engineFactor = GetEngineFactor();

        if (Mathf.Abs(thrustInput) > 0.01f)
            rb.AddForce(transform.up * (thrustInput * attributes.maxThrust * engineFactor * forwardThrustFactor));

        if (Mathf.Abs(strafeInput) > 0.01f)
            rb.AddForce(transform.right * (strafeInput * attributes.strafeThrust * engineFactor));
    }

    private void ClampSpeed()
    {
        Vector2 v = rb.linearVelocity;
        if (v.magnitude > attributes.maxSpeed)
            rb.linearVelocity = v.normalized * attributes.maxSpeed;
    }

    private void ClampToWorldBounds()
    {
        Vector2 pos = rb.position;

        pos.x = Mathf.Clamp(pos.x, worldMin.x, worldMax.x);
        pos.y = Mathf.Clamp(pos.y, worldMin.y, worldMax.y);

        rb.position = pos;
    }
}
