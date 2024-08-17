using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "ScriptableObjects/Block", order = 1)]
public class BlockSO : ScriptableObject
{
    public BlockType Type = BlockType.SQUARE_ONE;
    public int Cost = 10;
    public Sprite Icon;
    public GameObject Prefab;

    public void SelectBlock()
    {
        // TODO:: Call method to select block for build mode
        Debug.Log("Block Selected: " + Type.ToString());
    }
}

public enum BlockType
{
    SQUARE_ONE,
    VERTICAL_THREEE,
    HORIZONTAL_THREE,
    CROSS
}