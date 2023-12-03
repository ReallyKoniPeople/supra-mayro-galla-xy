using Assets;
using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerStarBehaviour : MonoBehaviour
{
    public string CurrentLevel;
    private bool StarCollected = false;

    public AudioSource audioSource;
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

            PlayerStats.Lives++;
            StarCollected = true;

            if (CurrentLevel == "Level_Boss")
            {
                SceneManager.LoadScene("GameUnder");
            }
            else
            {
                SceneManager.LoadScene("LevelSelection");            
            }

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioSource.clip, 1f);
            }
            StartCoroutine(WaitForSoundFinished());
        }
    }

    private IEnumerator WaitForSoundFinished()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
