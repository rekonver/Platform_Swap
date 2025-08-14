using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnLevelComplete += LoadNextLevel;
        GameEvents.OnLevelRestart += RestartCurrentLevel;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelComplete -= LoadNextLevel;
        GameEvents.OnLevelRestart -= RestartCurrentLevel;
    }

    private void LoadNextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            Debug.Log("Це останній рівень!");
        }
    }

    private void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
