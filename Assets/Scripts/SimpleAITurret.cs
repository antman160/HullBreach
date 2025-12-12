using UnityEngine;

public class SimpleAITurret : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Transform player;

    [Header("Rotation Settings")]
    public float rotateSpeed = 180f; 

    [Header("Firing Settings")]
    public float fireCooldown = 0.5f;
    public float fireAngleThreshold = 10f; 

    private float lastFireTime = -999f;

    void Update()
    {
        if (player == null) return;

        RotateTowardPlayer();

        if (IsAimedAtPlayer())
            TryFire();
    }

    void RotateTowardPlayer()
    {
        Vector3 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        float step = rotateSpeed * Time.deltaTime;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, step);
    }

    bool IsAimedAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        float currentAngle = transform.eulerAngles.z;

        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

        return angleDiff < fireAngleThreshold;
    }

    void TryFire()
    {
        if (Time.time < lastFireTime + fireCooldown)
            return;

        Fire();
        lastFireTime = Time.time;
    }

    void Fire()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject obj = GameObject.Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Projectile2D proj = obj.GetComponent<Projectile2D>();
        if (proj != null)
            proj.SetDirection(firePoint.up);  // FIRE ALONG UP AXIS
    }
}
