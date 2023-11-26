using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        PlayerStats.Lives = 3;
        SceneManager.LoadScene("MainMenu");
    }
}
