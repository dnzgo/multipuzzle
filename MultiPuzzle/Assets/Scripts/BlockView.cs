using UnityEngine;

public class BlockView : MonoBehaviour
{
    public Block Block { get; private set; }

    private BoxCollider2D boxCollider2D;

    public void Initialize(Block block)
    {
        Block = block;
        boxCollider2D = GetComponent<BoxCollider2D>();
        FitCollider();
    }

    private void FitCollider()
    {
        if (boxCollider2D == null) return;
        int blockWidth = Block.Width;
        int blockHeight = Block.Height;

        boxCollider2D.size = new Vector2(blockWidth, blockHeight);
        boxCollider2D.offset = new Vector2(0,0);
    }
}