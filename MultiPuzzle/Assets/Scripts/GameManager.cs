using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LevelData levelData;
    [SerializeField] private BoardRenderer renderer;
    [SerializeField] private BlockSpawner blockSpawner;
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
    }

    public Board GetBoard()
    {
        return board;
    }
}