using UnityEngine;

// Global ammo type enum
public enum ProjectileType
{
    Normal,
    ArmorPiercing
}

public class Projectile2D : MonoBehaviour
{
    [Header("Projectile Settings")]
    public ProjectileType type = ProjectileType.Normal;
    public float speed = 20f;
    public float lifeTime = 3f;
    public float damage = 20f;

    [Header("AP Behavior")]
    public float postPenDamageMultiplier = 0.5f;

    private Rigidbody2D rb;
    private bool hasPenetrated = false;
    private Vector2 travelDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Default direction is the projectile's forward (up)
        if (travelDir == Vector2.zero)
            travelDir = transform.up;

        rb.linearVelocity = travelDir * speed;

        // Auto-destroy after lifetime
        Destroy(gameObject, lifeTime);
    }

    // Called by turret when projectile is spawned
    public void SetDirection(Vector2 dir)
    {
        travelDir = dir.normalized;
        rb.linearVelocity = travelDir * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Prevent hitting same-layer projectiles
        if (other.gameObject.layer == gameObject.layer)
            return;

        // 1) ARMOR HIT
        ArmorPlate2D armor = other.GetComponent<ArmorPlate2D>();
        if (armor != null)
        {
            HandleArmorHit(armor);
            return;
        }

        // 2) SUBSYSTEM HIT
        Subsystem subsystem = other.GetComponent<Subsystem>();
        if (subsystem != null)
        {
            subsystem.TakeDamage(damage);
            HandlePostHit();
            return;
        }

        // 3) HULL HIT
        Health hp = other.GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
            HandlePostHit();
            return;
        }

        // Anything else breaks projectile
        DestroySelf();
    }

    // ------------------------------------
    //     ARMOR HIT RESOLUTION
    // ------------------------------------
    private void HandleArmorHit(ArmorPlate2D armor)
    {
        // HE (Normal) rounds stop
        if (type == ProjectileType.Normal)
        {
            armor.TakeArmorDamage(damage);
            DestroySelf();
            return;
        }

        // AP rounds
        if (!hasPenetrated)
        {
            // First penetration
            armor.TakeArmorDamage(damage);

            // Reduce damage for next hit
            damage *= postPenDamageMultiplier;

            hasPenetrated = true;

            // Slow bullet slightly
            rb.linearVelocity = travelDir * (speed * 0.75f);

            // Continue to next object
            return;
        }
        else
        {
            // Second armor hit = stop
            armor.TakeArmorDamage(damage);
            DestroySelf();
        }
    }

    // ------------------------------------
    //  AFTER ARMOR/SUBSYSTEM/HULL DAMAGE
    // ------------------------------------
    private void HandlePostHit()
    {
        // AP: First hit -> continue with reduced damage
        if (type == ProjectileType.ArmorPiercing && !hasPenetrated)
        {
            damage *= postPenDamageMultiplier;
            hasPenetrated = true;

            // Slow bullet slightly
            rb.linearVelocity = travelDir * (speed * 0.75f);

            return;
        }

        // HE or AP(2nd hit): stop
        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
