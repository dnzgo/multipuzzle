using UnityEngine;

public class Board
{
    private int width;
    private int height;
    private Cell[,] cells;

    public int Width => width;
    public int Height => height;

    public Board(LevelData levelData)
    {
        width = levelData.width;
        height = levelData.height;

        cells = new Cell[width, height];

        Initialize(levelData);
    }

    private void Initialize(LevelData levelData)
    {
        CellState[,] grid = LevelParser.Parse(
            levelData.layout,
            width,
            height
        );

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new Cell(
                    new Vector2Int(x, y),
                    grid[x, y]
                );
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (IsOutOfBounds(x, y)) return null;
        return cells[x, y];
    }

    public void SetCellState(int x, int y, CellState state)
    {
        if (IsOutOfBounds(x, y)) return;
        cells[x, y].SetState(state);
    }

    public bool IsOutOfBounds(int x, int y)
    {
        return x < 0 || y < 0 || x >= width || y >= height;
    }

    public bool IsCompleted()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (cells[x, y].State == CellState.SHAPE)
                    return false;
            }
        }
        return true;
    }
}