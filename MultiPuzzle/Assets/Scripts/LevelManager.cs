using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private LevelData currentLevel;

    private void Awake()
    {
        Instance = this;
    }

    public void StartLevel(LevelData level)
    {
        currentLevel = level;

        GameStateManager.Instance.EnableGameplay();

        //GameManager.Instance.LoadLevel(level);
    }

    public void RetryLevel()
    {
        Debug.Log("Retry Level");

        StartLevel(currentLevel);
    }

    public void NextLevel(LevelData nextLevel)
    {
        Debug.Log("Next Level");

        StartLevel(nextLevel);
    }
}