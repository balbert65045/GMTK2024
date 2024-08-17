using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField] private int _gridWidth = 16;
    [SerializeField] private int _gridHeight = 9;
    [SerializeField] private float _tileSize = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        // Calculate the center position
        float gridWidth = _gridWidth * _tileSize;
        float gridHeight = _gridHeight * _tileSize;

        // Calculate the starting positions to center the grid
        float startX = -(gridWidth / 2.0f) + (_tileSize / 2.0f);
        float startY = -(gridHeight / 2.0f) + (_tileSize / 2.0f);

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                // Create a new GameObject
                GameObject gridTile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                gridTile.transform.parent = transform;
                gridTile.transform.position = new Vector3(startX + x * _tileSize, startY + y * _tileSize, 0);

                // attach script to grid tile
                gridTile.AddComponent<GridTile>();

                // Adjust the scale of the tile to fit the desired size
                gridTile.transform.localScale = new Vector3(_tileSize, _tileSize, 1);
            }
        }
    }
}