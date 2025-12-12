using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(ShipAttributes))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private ShipAttributes attributes;

    [Header("Subsystem References")]
    public EngineSubsystem[] engines;

    [Header("World Bounds")]
    public Vector2 worldMin = new Vector2(-40f, -40f);
    public Vector2 worldMax = new Vector2(40f, 40f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attributes = GetComponent<ShipAttributes>();

        if (engines == null || engines.Length == 0)
            engines = GetComponentsInChildren<EngineSubsystem>();

        rb.gravityScale = 0f;
        rb.mass = attributes.mass;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        ClampSpeed();
        ApplyInertiaDamping();
    }

    private void LateUpdate()
    {
        ClampToWorldBounds();
    }

    private float GetEngineFactor()
    {
        if (engines == null || engines.Length == 0)
            return 1f;

        int destroyed = 0;

        foreach (var eng in engines)
        {
            if (!eng.IsOnline)
                destroyed++;
        }

        if (destroyed == 0)
            return 1f;

        if (destroyed == 1)
            return 0.75f;

        return 0.5f;
    }

    private void HandleMovement()
    {
        float thrustInput = 0f;
        if (Input.GetKey(KeyCode.W)) thrustInput = 1f;
        else if (Input.GetKey(KeyCode.S)) thrustInput = -1f;

        float strafeInput = 0f;
        if (Input.GetKey(KeyCode.D)) strafeInput = 1f;
        else if (Input.GetKey(KeyCode.A)) strafeInput = -1f;

        float engineFactor = GetEngineFactor();

        Vector2 forwardForce = (Vector2)transform.up * (thrustInput * attributes.maxThrust * engineFactor);
        rb.AddForce(forwardForce, ForceMode2D.Force);

        Vector2 lateralForce = (Vector2)transform.right * (strafeInput * attributes.strafeThrust * engineFactor);
        rb.AddForce(lateralForce, ForceMode2D.Force);
    }

    private void HandleRotation()
    {
        float yaw = 0f;
        if (Input.GetKey(KeyCode.E)) yaw = 1f;
        else if (Input.GetKey(KeyCode.Q)) yaw = -1f;

        float currentAngular = rb.angularVelocity;
        float targetAngular = -yaw * (attributes.turnTorque / attributes.mass);

        float torque = targetAngular - currentAngular;
        rb.AddTorque(torque, ForceMode2D.Force);
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

    private void ApplyInertiaDamping()
    {
        bool thrusting =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D);

        if (!thrusting)
        {
            float dampingFactor = 0.985f;
            Vector2 v = rb.linearVelocity;
            v *= dampingFactor;
            rb.linearVelocity = v;
        }
    }
}
