using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        
        var musicPlayers = FindObjectsByType<MenuMusic>(FindObjectsSortMode.None);
        if (musicPlayers.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
