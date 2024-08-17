using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField] private int _gridWidth = 16;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private float _tileSize = 1.0f;
    [SerializeField] private float _gridScale = 0.98f;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {

    float gridWidth = _gridWidth * _tileSize;
    float gridHeight = _gridHeight * _tileSize;

    float startX = -(gridWidth / 2) + (_tileSize / 2);
    float startY = -(gridHeight / 2) + (_tileSize / 2);

    for (int x = 0; x < _gridWidth; x++)
    {
        for (int y = 0; y < _gridHeight; y++)
        {
            // create grid tiles
            GameObject gridTile = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gridTile.transform.parent = transform;
            gridTile.transform.position = new Vector3(startX + x * _tileSize, startY + y * _tileSize, 0);

            // attach script to grid tile
            gridTile.AddComponent<GridTile>();

            // set the scale
            gridTile.transform.localScale = new Vector3(_tileSize * _gridScale, _tileSize * _gridScale, 1);
        }
    }
}
}