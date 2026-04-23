using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LevelData levelData;
    [SerializeField] private BoardRenderer renderer;
    [SerializeField] private BlockSpawner blockSpawner;
    [SerializeField] private LevelTimer levelTimer;
    private Block[] blocks;

    private Board board;

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        board = new Board(levelData);
        board.SetWorldOrigin(renderer.GetBoardWorldOrigin());
        renderer.Render(board);

        blocks = blockSpawner.SpawnBlocks(levelData);

        levelTimer.OnTimeUp += HandleTimeUp;
    }

    public Board GetBoard()
    {
        return board;
    }

    public void StartLevelTimerIfNeeded()
    {
        levelTimer.StartTimerOnFirstTouch(levelData.timeLimit);
    }

    private void HandleTimeUp()
    {
        Debug.Log("Time's up!");

        levelTimer.StopTimer();
        GameStateManager.Instance.DisableGameplay();

        // force stop all dragging because Player is dragging → timer hits 0 → OnMouseUp still fires → block might place AFTER fail
        foreach (var drag in FindObjectsByType<DragHandler>(FindObjectsSortMode.None))
        {
            drag.enabled = false;
        }

        // TODO: lose condition
        // show fail UI
    }

    public void HandleLevelCompleted()
    {
        Debug.Log("LEVEL COMPLETE");

        levelTimer.StopTimer();
        GameStateManager.Instance.DisableGameplay();

        // force stop all dragging
        foreach (var drag in FindObjectsByType<DragHandler>(FindObjectsSortMode.None))
        {
            drag.enabled = false;
        }

        // later:
        // show win UI
        // load next level
    }
    
}