using UnityEngine;

public class ShipAimController : MonoBehaviour
{
    [Header("General")]
    public Camera cam;

    [Tooltip("How far from the ship the reticle / aim point sits.")]
    public float aimRadius = 4f;

    [Tooltip("Optional reticle transform to move around the ship.")]
    public Transform reticleTransform;

    // Public read-only values for other scripts (turrets, etc.)
    public Vector2 AimDirection { get; private set; }      // normalized direction
    public Vector2 AimWorldPosition { get; private set; }  // world position of reticle

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    private void Update()
    {
        if (cam == null) return;

        // 1. Get mouse position in world space
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = transform.position.z;

        // 2. Direction from ship to mouse
        Vector2 dir = (Vector2)(mouseWorld - transform.position);

        if (dir.sqrMagnitude < 0.0001f)
        {
            // If mouse is exactly on ship, default to ship's forward (up)
            AimDirection = transform.up;
        }
        else
        {
            AimDirection = dir.normalized;
        }

        // 3. Clamp to a fixed radius -> "virtual joystick ring"
        AimWorldPosition = (Vector2)transform.position + AimDirection * aimRadius;

        // 4. Move optional reticle object
        if (reticleTransform != null)
        {
            reticleTransform.position = AimWorldPosition;
            reticleTransform.up = AimDirection; 
        }
    }
}
