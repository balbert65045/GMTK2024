using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType {
    SQUARE,
    VERTICAL_THREE,
    HORIZONTAL_THREE,
    CROSS,
    L_SHAPE_LEFT,
    L_SHAPE_RIGHT,
}

public class Block : MonoBehaviour
{
    public BlockType blockType;


    public List<Vector2> GetNeighbors()
    {
        List<Vector2> _neighbors = new List<Vector2>();

        switch (blockType)
        {
            case BlockType.VERTICAL_THREE:
                _neighbors.Add(new Vector2(0, 1));
                _neighbors.Add(new Vector2(0, -1));
                break;
            case BlockType.HORIZONTAL_THREE:
                _neighbors.Add(new Vector2(1, 0));
                _neighbors.Add(new Vector2(-1, 0));
                break;
            case BlockType.CROSS:
                _neighbors.Add(new Vector2(0, 1));
                _neighbors.Add(new Vector2(0, -1));
                _neighbors.Add(new Vector2(1, 0));
                _neighbors.Add(new Vector2(-1, 0));
                break;
            case BlockType.L_SHAPE_LEFT:
                _neighbors.Add(new Vector2(0, 1));
                _neighbors.Add(new Vector2(0, -1));
                _neighbors.Add(new Vector2(-1, -1));
                break;
            case BlockType.L_SHAPE_RIGHT:
                _neighbors.Add(new Vector2(0, 1));
                _neighbors.Add(new Vector2(0, -1));
                _neighbors.Add(new Vector2(1, -1));
                break;
        }

        return _neighbors;
    }
}
