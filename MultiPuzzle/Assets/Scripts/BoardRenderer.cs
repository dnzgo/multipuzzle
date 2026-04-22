using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform parent;

    private GameObject[,] visuals;

    public Vector3 GetBoardWorldOrigin()
    {
        return parent != null ? parent.position : transform.position;
    }

    public void Render(Board board)
    {
        visuals = new GameObject[board.Width, board.Height];

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                Cell cell = board.GetCell(x, y);

                GameObject obj = Instantiate(cellPrefab, parent);
                float offsetX = (board.Width - 1) / 2f;
                float offsetY = (board.Height - 1) / 2f;

                obj.transform.localPosition = new Vector3(
                    x - offsetX,
                    y - offsetY,
                    0
                );

                ApplyVisual(obj, cell.State);

                visuals[x, y] = obj;
            }
        }
    }

    public void UpdateCell(int x, int y, CellState state)
    {
        GameObject obj = visuals[x, y];
        ApplyVisual(obj, state);
    }

    private void ApplyVisual(GameObject obj, CellState state)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        switch (state)
        {
            case CellState.EMPTY:
                sr.enabled = false;
                break;

            case CellState.SHAPE:
                sr.enabled = true;
                sr.color = Color.gray;
                break;

            case CellState.FILLED:
                sr.enabled = true;
                sr.color = Color.green;
                break;
        }
    }
}