using UnityEngine;
using static Projectile2D;

public class ShipWeapons : MonoBehaviour
{
    [Header("Ammo Settings")]
    public ProjectileType currentAmmo = ProjectileType.Normal;
    public KeyCode switchAmmoKey = KeyCode.R;

    [Header("Ammo Balancing")]
    public float normalDamage = 15f;
    public float normalSpeed = 60f;

    public float apDamage = 8f;
    public float apSpeed = 70f;

    private void Update()
    {
        HandleAmmoSwitch();
    }

    private void HandleAmmoSwitch()
    {
        if (Input.GetKeyDown(switchAmmoKey))
        {
            if (currentAmmo == ProjectileType.Normal)
                currentAmmo = ProjectileType.ArmorPiercing;
            else
                currentAmmo = ProjectileType.Normal;

            Debug.Log("Switched ammo to: " + currentAmmo);
        }
    }

    // Applies ammo settings to newly-spawned projectiles
    public void ConfigureProjectile(Projectile2D proj)
    {
        if (currentAmmo == ProjectileType.ArmorPiercing)
        {
            proj.type = ProjectileType.ArmorPiercing;
            proj.damage = apDamage;
            proj.speed = apSpeed;
        }
        else
        {
            proj.type = ProjectileType.Normal;
            proj.damage = normalDamage;
            proj.speed = normalSpeed;
        }
    }
}
