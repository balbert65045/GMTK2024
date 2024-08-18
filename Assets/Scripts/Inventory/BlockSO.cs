using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "ScriptableObjects/Block", order = 1)]
public class BlockSO : ScriptableObject
{
    public BlockType Type = BlockType.HORIZONTAL_THREE;
    public int Cost = 10;
    public Sprite Icon;
    public GameObject Prefab;
}