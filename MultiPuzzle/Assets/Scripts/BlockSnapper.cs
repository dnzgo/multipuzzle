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
        Vector2Int baseGrid;

        // 1. check if close enough to snap
        if (!TryGetSnapGrid(blockRoot, out baseGrid))
            return false;

        // 2. validate placement
        if (!CanPlace(blockRoot, baseGrid))
            return false;

        // 3. place block
        Place(blockRoot, baseGrid);

        return true;
    }

    private bool TryGetSnapGrid(Transform blockRoot, out Vector2Int gridPos)
    {
        gridPos = Vector2Int.zero;

        Transform anchor = blockRoot.GetChild(0);

        Vector3 worldPos = anchor.position;

        gridPos = Board.WorldToGrid(worldPos);

        Vector3 gridWorld = Board.GridToWorld(gridPos.x, gridPos.y);

        float distance = Vector2.Distance(worldPos, gridWorld);

        float snapThreshold = 0.3f;

        return distance <= snapThreshold;
    }

        private bool CanPlace(Transform blockRoot, Vector2Int baseGrid)
    {
        Transform anchor = blockRoot.GetChild(0);

        foreach (Transform cell in blockRoot)
        {
            Vector2Int cellGrid = Board.WorldToGrid(cell.position);

            Vector2Int offset = cellGrid - Board.WorldToGrid(anchor.position);
            Vector2Int finalPos = baseGrid + offset;

            if (Board.IsOutOfBounds(finalPos.x, finalPos.y))
                return false;

            var boardCell = Board.GetCell(finalPos.x, finalPos.y);

            if (boardCell.State != CellState.SHAPE)
                return false;
        }

        return true;
    }

        private void Place(Transform blockRoot, Vector2Int baseGrid)
    {
        Transform anchor = blockRoot.GetChild(0);

        foreach (Transform cell in blockRoot)
        {
            Vector2Int cellGrid = Board.WorldToGrid(cell.position);

            Vector2Int offset = cellGrid - Board.WorldToGrid(anchor.position);
            Vector2Int finalPos = baseGrid + offset;

            Board.SetCellState(finalPos.x, finalPos.y, CellState.FILLED);
        }

        SnapVisual(blockRoot, baseGrid);
    }

        private void SnapVisual(Transform blockRoot, Vector2Int gridPos)
    {
        Transform anchor = blockRoot.GetChild(0);

        Vector3 target = Board.GridToWorld(gridPos.x, gridPos.y);
        Vector3 delta = target - anchor.position;

        blockRoot.position += delta;
    }

}