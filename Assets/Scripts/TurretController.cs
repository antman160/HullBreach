using UnityEngine;

public class TurretController : MonoBehaviour
{
    public ShipAimController aimSource;
    public float traverseSpeed = 90f;

    public Subsystem turretSubsystem; // <-- must exist

    private void Awake()
    {
        if (turretSubsystem == null)
            turretSubsystem = GetComponentInParent<Subsystem>();
    }

    private void Update()
    {
        if (turretSubsystem != null && turretSubsystem.isDestroyed)
            return;

        if (aimSource == null)
            return;

        Vector2 aimDir = aimSource.AimDirection;
        if (aimDir.sqrMagnitude < 0.0001f)
            return;

        float targetAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = transform.eulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            traverseSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }
}
