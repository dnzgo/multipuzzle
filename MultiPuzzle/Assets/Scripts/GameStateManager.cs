using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool IsPaused { get; private set; }
    public bool IsGameplayActive { get; private set; } = true;

    private void Awake()
    {
        Instance = this;
    }

    public void PauseGame()
    {
        IsPaused = true;
        IsGameplayActive = false;

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        IsGameplayActive = true;

        Time.timeScale = 1f;
    }

    public void DisableGameplay()
    {
        IsGameplayActive = false;
    }

    public void EnableGameplay()
    {
        IsGameplayActive = true;
    }
}