using UnityEngine;

public class Block
{
    public BlockData data;

    public int Width => data.width;
    public int Height => data.height;

    public bool[,] GetShape()
    {
        bool[,] result = new bool[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int index = x + y * Width;
                result[x, y] = data.shape[index] == 1;
            }
        }

        return result;
    }
}