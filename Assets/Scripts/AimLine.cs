using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(ShipAimController))]
public class AimLine : MonoBehaviour
{
    private LineRenderer lr;
    private ShipAimController aim;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        aim = GetComponent<ShipAimController>();
    }

    private void Update()
    {
        if (aim == null) return;

        // From ship center to aim position
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, aim.AimWorldPosition);
    }
}
