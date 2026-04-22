using UnityEngine;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject blockCellPrefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private float previewSpacing = 1.5f;
    [SerializeField] private float targetPreviewHeight = 0.5f;

    private readonly List<DragHandler> previewOrder = new List<DragHandler>();

    public Block[] SpawnBlocks(LevelData level)
    {
        Block[] blocks = new Block[level.blocks.Length];

        for (int i = 0; i < level.blocks.Length; i++)
        {
            GameObject obj = Instantiate(blockPrefab, spawnParent);

            Block block = new Block
            {
                data = level.blocks[i]
            };

            BlockView view = obj.GetComponent<BlockView>();
            view.Initialize(block);

            obj.name = $"Block_{i}";

            blocks[i] = block;

            DrawBlockVisual(obj.transform, block);
            ConfigureBlockScales(obj, block);
            RegisterToPreview(obj);

        }

        return blocks;
    }

    public bool RemoveFromPreview(DragHandler handler)
    {
        if (handler == null) return false;
        bool removed = previewOrder.Remove(handler);
        if (!removed) return false;

        RefreshPreviewPositions();
        return true;
    }

    public bool ReturnToPreview(DragHandler handler)
    {
        if (handler == null) return false;
        if (previewOrder.Contains(handler))
        {
            RefreshPreviewPositions();
            return true;
        }

        previewOrder.Add(handler);
        RefreshPreviewPositions();
        return true;
    }

    private void DrawBlockVisual(Transform parent, Block block)
    {
        var shape = block.GetShape();

        float offsetX = (block.Width - 1) / 2f;
        float offsetY = (block.Height - 1) / 2f;

        for (int y = 0; y < block.Height; y++)
        {
            for (int x = 0; x < block.Width; x++)
            {
                if (!shape[x, y]) continue;

                GameObject cell = Instantiate(blockCellPrefab, parent);
                cell.transform.SetParent(parent);
                cell.transform.localPosition = new Vector3(
                    x - offsetX,
                    y - offsetY,
                    0
                );

                cell.transform.localScale = Vector3.one * 1.0f;
            }
        }
    }

    private void ConfigureBlockScales(GameObject blockObject, Block block)
    {
        DragHandler dragHandler = blockObject.GetComponent<DragHandler>();
        if (dragHandler == null) return;

        int shapeHeight = Mathf.Max(1, block.Height);
        float previewScale = targetPreviewHeight / shapeHeight;
        float dragScale = 1f;

        dragHandler.SetScales(previewScale, dragScale);
    }

    private void RegisterToPreview(GameObject blockObject)
    {
        DragHandler dragHandler = blockObject.GetComponent<DragHandler>();
        if (dragHandler == null) return;
        if (previewOrder.Contains(dragHandler)) return;

        previewOrder.Add(dragHandler);
        RefreshPreviewPositions();
    }

    private void RefreshPreviewPositions()
    {
        for (int i = previewOrder.Count - 1; i >= 0; i--)
        {
            if (previewOrder[i] == null)
                previewOrder.RemoveAt(i);
        }

        for (int i = 0; i < previewOrder.Count; i++)
        {
            previewOrder[i].SetPreviewPosition(GetSlotWorldPosition(i));
        }
    }

    private Vector3 GetSlotWorldPosition(int index)
    {
        float y = spawnParent != null ? spawnParent.position.y : transform.position.y;
        float x = (spawnParent != null ? spawnParent.position.x : transform.position.x) + (index * previewSpacing);
        return new Vector3(x, y, 0f);
    }
}