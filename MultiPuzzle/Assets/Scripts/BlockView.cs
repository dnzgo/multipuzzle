using UnityEngine;

public class BlockView : MonoBehaviour
{
    public Block Block { get; private set; }

    public void Initialize(Block block)
    {
        Block = block;
    }
}