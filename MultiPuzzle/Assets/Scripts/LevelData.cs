using UnityEngine;

[CreateAssetMenu (fileName = "LevelData", menuName = "Puzzle/LevelData")]
public class LevelData : ScriptableObject
{
    public int width, height;

    [TextArea(5, 10)]
    public string layout;
    public BlockData[] blocks;
}
