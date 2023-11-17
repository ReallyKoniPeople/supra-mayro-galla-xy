using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerStarBehaviour : MonoBehaviour
{
    public string CurrentLevel;
    private bool StarCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !StarCollected)
        {
            if (!LevelStats.Level1Cleared && CurrentLevel == "Level_1")
            {
                LevelStats.Level1Cleared = true;
            }
            else if (!LevelStats.Level2Cleared && CurrentLevel == "Level_2")
            {
                LevelStats.Level2Cleared = true;
            }
            else if (!LevelStats.LevelBossCleared && CurrentLevel == "Level_Boss")
            {
                LevelStats.LevelBossCleared = true;
            }

            SceneManager.LoadScene("LevelSelection");
            StarCollected = true;
        }
    }
}
