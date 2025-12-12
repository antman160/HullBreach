using UnityEngine;
using UnityEngine.UI;

public class PlayerMapMovement : MonoBehaviour
{
    public RectTransform mapRect;
    public RectTransform playerIcon;
    public RectTransform targetMarker;
    public float moveSpeed = 300f;
    public UILine pathLine;


    private Vector2 targetPos;
    private bool moving = false;

    void Update()
    {
        // Handle click on map
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                mapRect,
                Input.mousePosition,
                null,
                out localPoint))
            {
                targetMarker.anchoredPosition = localPoint;
                targetPos = localPoint;
                moving = true;

                if (pathLine != null)
                    pathLine.showLine = true;
            }
        }

        // Handle movement 
        if (moving)
        {
            playerIcon.anchoredPosition =
                Vector2.MoveTowards(playerIcon.anchoredPosition, targetPos, moveSpeed * Time.deltaTime);

            // ROTATE TOWARD TARGET
            Vector2 dir = targetPos - playerIcon.anchoredPosition;
            if (dir.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                playerIcon.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }

            // ARRIVAL CHECK
            if (Vector2.Distance(playerIcon.anchoredPosition, targetPos) < 0.1f)
            {
                moving = false;

                if (pathLine != null)
                    pathLine.showLine = false;
            }
        }
    }


    
    public Vector2 GetPlayerPosition()
    {
        return playerIcon.anchoredPosition;
    }


    public bool AtNode(EncounterNode node)
    {
        float dist = Vector2.Distance(playerIcon.anchoredPosition, node.nodePosition);
        return dist <= node.requiredDistance;
    }
}
