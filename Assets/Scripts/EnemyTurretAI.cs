using UnityEngine;

public class EnemyTurretAI : MonoBehaviour
{
    public Transform[] firePoints;
    public GameObject projectilePrefab;

    public Subsystem turretSubsystem;
    public ShipWeapons ammoManager;

    public float traverseSpeed = 60f;
    public float fireCooldown = 0.4f;

    private float lastFireTime;
    public Transform target;

    private void Update()
    {
        if (turretSubsystem != null && turretSubsystem.isDestroyed)
            return;

        if (target == null)
            return;

        Vector2 aimDir = target.position - transform.position;

        float targetAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;

        float newAngle = Mathf.MoveTowardsAngle(
            transform.eulerAngles.z,
            targetAngle,
            traverseSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

        if (Time.time >= lastFireTime + fireCooldown)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    private void Fire()
    {
        foreach (Transform fp in firePoints)
        {
            GameObject obj = Instantiate(projectilePrefab, fp.position, fp.rotation);
            Projectile2D proj = obj.GetComponent<Projectile2D>();

            if (proj != null && ammoManager != null)
                ammoManager.ConfigureProjectile(proj);

            proj.SetDirection(fp.up);
        }
    }
}

