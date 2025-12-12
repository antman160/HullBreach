using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("TacticalMapScene"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
