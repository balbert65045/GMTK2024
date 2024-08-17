using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private bool _isSelected = true;
    public int tileWidthCount = 1;
    public int tileHeightCount = 1;
    private bool _isDragging = false;
    private GameObject[] _gridTiles;

    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);

        StartCoroutine(InitGridTiles());
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (_isDragging && _isSelected)
        {
            FindSelectedTile();
        }
    }

    void FindSelectedTile()
    {
        foreach (GameObject gridTile in _gridTiles)
        {
            GridTile gridScript = gridTile.GetComponent<GridTile>();
            bool isSelected = gridScript.IsSelected();
            if (isSelected == true)
            {
                // set the selected tile to the grid tile
                transform.position = gridTile.transform.position;
            }
        }       
    }

    IEnumerator InitGridTiles()
    {
        yield return new WaitForSeconds(0.5f);
        _gridTiles = GameObject.FindGameObjectsWithTag("GridTile");
    }

    public bool IsSelected()
    {
        return _isSelected;
    }
}
