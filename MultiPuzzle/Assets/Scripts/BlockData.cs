[System.Serializable]
public class BlockData
{
    public string id;
    public int width;
    public int height;

    // 0 = empty, 1 = block cell
    public int[] shape;
}