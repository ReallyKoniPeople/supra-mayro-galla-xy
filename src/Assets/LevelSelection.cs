using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (LevelStats.Level1Cleared)
        {
            var level2Button = GameObject.Find("Level2Button").GetComponent<Button>();
            level2Button.interactable = true;
        }
        if (LevelStats.Level2Cleared)
        {
            var levelBossButton = GameObject.Find("LevelBossButton").GetComponent<Button>();
            levelBossButton.interactable = true;
        }
    }

    public void SelectLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
