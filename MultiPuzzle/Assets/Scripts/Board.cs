using UnityEngine;

public class Board
{
    private int width;
    private int height;
    private Cell[,] cells;
    private Vector3 worldOrigin;

    public int Width => width;
    public int Height => height;

    public Board(LevelData levelData)
    {
        width = levelData.width;
        height = levelData.height;
        worldOrigin = Vector3.zero;

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

    public void SetWorldOrigin(Vector3 origin)
    {
        worldOrigin = origin;
    }

    public Vector3 GridToWorld(int x, int y)
    {
        float offsetX = (width - 1) / 2f;
        float offsetY = (height - 1) / 2f;

        return new Vector3(
            worldOrigin.x + x - offsetX,
            worldOrigin.y + y - offsetY,
            0
        );
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        float offsetX = (width - 1) / 2f;
        float offsetY = (height - 1) / 2f;

        int x = Mathf.RoundToInt((worldPos.x - worldOrigin.x) + offsetX);
        int y = Mathf.RoundToInt((worldPos.y - worldOrigin.y) + offsetY);

        return new Vector2Int(x, y);
    }

}