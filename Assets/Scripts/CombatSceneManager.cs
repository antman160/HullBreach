using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatSceneManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string tacticalMapSceneName = "TacticalMapScene";
    public string mainMenuSceneName = "MainMenu";

    [Header("Check Settings")]
    public float checkInterval = 1f; // how often to check for enemies

    private float nextCheckTime = 0f;

    void Update()
    {
        HandleEscapeKey();

        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            CheckWinCondition();
        }
    }

    void HandleEscapeKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    void CheckWinCondition()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            SceneManager.LoadScene(tacticalMapSceneName);
        }
    }
}
