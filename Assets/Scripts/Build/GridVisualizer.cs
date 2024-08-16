using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField] private int gridSize = 10;
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int x = -10; x < gridSize; x++)
        {
            for (int y = -10; y < gridSize; y++)
            {
                // Create a new GameObject
                GameObject gridTile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                // Set the parent of the grid tile to the grid manager
                gridTile.transform.parent = transform;
                // Make the grid tile semi-transparent
                gridTile.GetComponent<Renderer>().material.color = new Color(0, 0, 1, 0.1f);
                // Set the position of the grid tile
                gridTile.transform.position = new Vector3(x, y, 0);
                // Set the scale of the grid tile
                gridTile.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            }
        }
    }
}
