using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Targets")]
    public Transform target;          // Player ship

    [Header("Follow")]
    public float smoothTime = 0.2f;   // Lower = snappier, higher = floaty

    [Header("Look Ahead (toward mouse / reticle)")]
    public float lookAheadDistance = 3f;   // How far ahead of the ship the camera leans
    public float lookAheadSmooth = 5f;     // How fast the look-ahead moves

    [Header("Static Offset")]
    public Vector3 baseOffset = new Vector3(0f, 0f, 0f); // XY offset from ship if you want

    [Header("Bounds")]
    public bool useBounds = true;
    public Vector2 boundsMin = new Vector2(-40f, -40f);  // left, bottom
    public Vector2 boundsMax = new Vector2(40f, 40);  // right, top

    private Vector3 _currentVelocity;     // For SmoothDamp
    private float _baseZ;                 // Camera's original Z
    private Vector3 _currentLookAhead;    // Smoothed look-ahead offset

    private Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _baseZ = transform.position.z; // usually -10
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // === 1. Direction from ship to mouse
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorld = _cam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = target.position.z;

        Vector2 toMouse = (mouseWorld - target.position);
        Vector3 targetLookAhead = Vector3.zero;

        if (toMouse.sqrMagnitude > 0.001f)
        {
            Vector2 dir = toMouse.normalized;
            targetLookAhead = (Vector3)(dir * lookAheadDistance);
        }

        // Smooth the look-ahead movement
        _currentLookAhead = Vector3.Lerp(_currentLookAhead, targetLookAhead, lookAheadSmooth * Time.deltaTime);

        // === 2. Desired camera position before clamping ===
        Vector3 desiredPos = target.position + baseOffset + _currentLookAhead;
        desiredPos.z = _baseZ;

        // === 3. Smoothly move camera toward that point ===
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, desiredPos, ref _currentVelocity, smoothTime);

        // === 4. Clamp to bounds (so camera never shows outside arena) ===
        if (useBounds)
        {
            float camHalfHeight = _cam.orthographicSize;
            float camHalfWidth = camHalfHeight * _cam.aspect;

            // Only clamp if the bounds are actually larger than the camera
            float minX = boundsMin.x + camHalfWidth;
            float maxX = boundsMax.x - camHalfWidth;
            float minY = boundsMin.y + camHalfHeight;
            float maxY = boundsMax.y - camHalfHeight;

            // If the arena is smaller than the camera
            if (minX < maxX && minY < maxY)
            {
                smoothedPos.x = Mathf.Clamp(smoothedPos.x, minX, maxX);
                smoothedPos.y = Mathf.Clamp(smoothedPos.y, minY, maxY);
            }
        }

        transform.position = smoothedPos;
    }
}
