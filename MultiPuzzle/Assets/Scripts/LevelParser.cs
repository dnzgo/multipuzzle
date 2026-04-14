using UnityEngine;

public static class LevelParser
{
    public static CellState[,] Parse(string layout, int width, int height)
    {
        CellState[,] result = new CellState[width, height];

        string[] rows = layout.Split('\n');

        for (int y = 0; y < height; y++)
        {
            string[] cols = rows[height - y -1].Trim().Split(' ');

            for (int x = 0; x < width; x++)
            {
                int value = int.Parse(cols[x]);

                result[x, y] = (CellState)value;
            }
        }

        return result;
    }
}