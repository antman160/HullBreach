using UnityEngine;

public class TurretFireController : MonoBehaviour
{
    public Transform[] firePoints;
    public GameObject projectilePrefab;

    public float fireCooldown = 0.3f;
    private float lastFireTime = -999f;

    public Subsystem turretSubsystem; 
    public ShipWeapons weaponManager; 

    public bool holdToFire = true;

    private void Update()
    {
        
        if (turretSubsystem != null && turretSubsystem.isDestroyed)
            return;

        bool wantsToFire;

        if (holdToFire)
            wantsToFire = Input.GetMouseButton(0);
        else
            wantsToFire = Input.GetMouseButtonDown(0);

        if (!wantsToFire)
            return;

        if (Time.time < lastFireTime + fireCooldown)
            return;

        Fire();
        lastFireTime = Time.time;
    }

    private void Fire()
    {
        if (projectilePrefab == null || firePoints == null)
            return;

        foreach (Transform fp in firePoints)
        {
            if (fp == null)
                continue;

            GameObject obj = Instantiate(projectilePrefab, fp.position, fp.rotation);
            Projectile2D proj = obj.GetComponent<Projectile2D>();

            if (proj != null && weaponManager != null)
            {
                
                weaponManager.ConfigureProjectile(proj);

                // give initial velocity
                proj.SetDirection(fp.up);
            }
        }
    }
}
