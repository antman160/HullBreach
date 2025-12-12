using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShipConfig", menuName = "Ships/EnemyShipConfig")]
public class EnemyShipConfig : ScriptableObject
{
    public float desiredCombatDistance = 20f;
    public float maxEngageRange = 40f;
    public float retreatThreshold = 0.3f;

    public float moveSpeedMultiplier = 1f;
    public float turnSpeedMultiplier = 1f;

    public float turretRange = 50f;
    public float fireCooldown = 0.5f;

    public int difficulty = 1;
}
