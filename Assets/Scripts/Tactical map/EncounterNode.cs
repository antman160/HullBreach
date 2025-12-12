using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EncounterNode : MonoBehaviour, IPointerClickHandler
{
    public Vector2 nodePosition;             // UI local map position
    public string combatSceneName = "CombatScene1";
    public float requiredDistance = 50f;     // Distance required before scene loads

    private PlayerMapMovement mapController;

    void Start()
    {
        // Position the node on the map
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
            rt.anchoredPosition = nodePosition;

        
        mapController = FindFirstObjectByType<PlayerMapMovement>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (mapController == null)
        {
            Debug.LogWarning("No PlayerMapMovement found.");
            return;
        }

        // Check distance
        if (!mapController.AtNode(this))
        {
            Debug.Log("Player is too far from node.");
            return;
        }

        // Load the combat scene
        Debug.Log("Entering combat: " + combatSceneName);
        SceneManager.LoadScene(combatSceneName);
    }
}
