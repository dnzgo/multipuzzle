using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private BoardRenderer renderer;

    private Board board;

    void Start()
    {
        board = new Board(levelData);
        renderer.Render(board);
    }
}