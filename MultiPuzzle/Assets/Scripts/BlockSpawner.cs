using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject blockCellPrefab;
    [SerializeField] private Transform spawnParent;

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

            obj.transform.position = new Vector3(i * 1.5f, spawnParent.transform.position.y, 0);
        }

        return blocks;
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
                parent.transform.localScale = Vector3.one * 0.5f;
            }
        }
    }
}