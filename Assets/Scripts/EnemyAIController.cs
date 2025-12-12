using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public EnemyShipMovement2D movement;
    public ShipAttributes attributes;
    public Transform player;

    private void Awake()
    {
        movement = GetComponent<EnemyShipMovement2D>();
        attributes = GetComponent<ShipAttributes>();

        // Auto-find player if none assigned
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    private void Update()
    {
        if (movement == null)
            return;

     
        movement.playerTarget = player;
    }
}
