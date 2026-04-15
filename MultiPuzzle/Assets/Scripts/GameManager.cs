using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private BoardRenderer renderer;
    [SerializeField] private BlockSpawner blockSpawner;
    private Block[] blocks;

    private Board board;

    void Start()
    {
        board = new Board(levelData);
        renderer.Render(board);

        blocks = blockSpawner.SpawnBlocks(levelData);
    }
}