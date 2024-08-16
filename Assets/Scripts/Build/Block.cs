using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private bool canBuild = true;
    [SerializeField] private bool isSelected = true;
    [SerializeField] private float gridSize = 1f;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && canBuild)
        {
            MoveBlockWithMouse();
        }
    }

    void MoveBlockWithMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z position is 0 since we're in 2D

        // Snap the block to the nearest grid position
        Vector3 snappedPosition = new Vector3(
            Mathf.Round(mousePosition.x / gridSize) * gridSize,
            Mathf.Round(mousePosition.y / gridSize) * gridSize,
            0
        );

        transform.position = snappedPosition;
    }
}
