using UnityEngine;

public class BlockSnapper : MonoBehaviour
{
    private Board Board
    {
        get
        {
            return GameManager.Instance != null
                ? GameManager.Instance.GetBoard()
                : null;
        }
    }

    public bool TrySnapAndPlace(Transform blockRoot)
    {
        Vector2Int[] _;
        return TrySnapAndPlace(blockRoot, out _);
    }

    public bool TrySnapAndPlace(Transform blockRoot, out Vector2Int[] placedCells)
    {
        placedCells = null;

        if (Board == null)
            return false;

        Vector2Int[] targetCells;

        // 1. snap each cell to nearest grid cell and validate placement
        if (!TryGetSnappedCells(blockRoot, out targetCells))
            return false;

        // 2. place block
        Place(blockRoot, targetCells);
        placedCells = targetCells;

        return true;
    }

    public void SetCellsState(Vector2Int[] cells, CellState state)
    {
        if (Board == null || cells == null)
            return;

        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int pos = cells[i];

            if (Board.IsOutOfBounds(pos.x, pos.y))
                continue;

            Board.SetCellState(pos.x, pos.y, state);
        }

        LogBoardStates($"Board cell states after SetCellsState -> {state}:");
    }

    private bool TryGetSnappedCells(Transform blockRoot, out Vector2Int[] targetCells)
    {
        int count = blockRoot.childCount;
        targetCells = new Vector2Int[count];

        for (int i = 0; i < count; i++)
        {
            Transform cell = blockRoot.GetChild(i);
            Vector2Int finalPos = Board.WorldToGrid(cell.position);

            if (Board.IsOutOfBounds(finalPos.x, finalPos.y))
            {
                targetCells = null;
                return false;
            }

            var boardCell = Board.GetCell(finalPos.x, finalPos.y);

            if (boardCell.State != CellState.SHAPE)
            {
                targetCells = null;
                return false;
            }

            targetCells[i] = finalPos;
        }

        return true;
    }

    private void Place(Transform blockRoot, Vector2Int[] targetCells)
    {
        for (int i = 0; i < targetCells.Length; i++)
        {
            Vector2Int finalPos = targetCells[i];
            Board.SetCellState(finalPos.x, finalPos.y, CellState.FILLED);
        }

        SnapVisual(blockRoot, targetCells);
        LogBoardStates("Board cell states after placement:");
    }

    private void SnapVisual(Transform blockRoot, Vector2Int[] targetCells)
    {
        Transform anchor = blockRoot.GetChild(0);
        Vector3 target = Board.GridToWorld(targetCells[0].x, targetCells[0].y);
        Vector3 delta = target - anchor.position;

        blockRoot.position += delta;
    }

    private void LogBoardStates(string title)
    {
        if (Board == null)
            return;

        Debug.Log($"{title} size={Board.Width}x{Board.Height}");

        for (int y = Board.Height - 1; y >= 0; y--)
        {
            string row = string.Empty;
            for (int x = 0; x < Board.Width; x++)
            {
                Cell cell = Board.GetCell(x, y);
                row += ToCellChar(cell != null ? cell.State : CellState.EMPTY);
                if (x < Board.Width - 1) row += " ";
            }
            Debug.Log($"row {y}: {row}");
        }
    }

    private char ToCellChar(CellState state)
    {
        switch (state)
        {
            case CellState.EMPTY: return 'E';
            case CellState.SHAPE: return 'S';
            case CellState.FILLED: return 'F';
            default: return '?';
        }
    }

}