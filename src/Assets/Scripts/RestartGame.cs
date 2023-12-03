using Assets;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        LevelStats.Level1Cleared = false;
        LevelStats.Level2Cleared = false;
        LevelStats.LevelBossCleared = false;
        PlayerStats.Lives = 3;
        SceneManager.LoadScene("MainMenu");
    }
}
