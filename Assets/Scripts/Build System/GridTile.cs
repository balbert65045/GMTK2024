using UnityEngine;

public class GridTile : MonoBehaviour
{

    private Renderer _renderer;
    private bool _isSelected = false;
    [SerializeField] private float _transparencyWhenHovered = 0.7f;
    [SerializeField] private float _transparencyWhenNotHovered = 0.5f;
    [SerializeField] private float _gridScale = 0.95f;

    void Start()
    {
        gameObject.tag = "GridTile";
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _renderer.material.shader = Shader.Find("Transparent/Diffuse");
            _renderer.material.color = new Color(0, 0, 1, _transparencyWhenNotHovered);
        }
        transform.localScale = new Vector3(_gridScale, _gridScale, 1);
    }

    void OnMouseEnter()
    {
        if (_renderer != null)
        {
            _renderer.material.color = new Color(0, 0, 1, _transparencyWhenHovered);
            _isSelected = true;
        }
    }

    void OnMouseExit()
    {
        if (_renderer != null)
        {
            _renderer.material.color = new Color(0, 0, 1, _transparencyWhenNotHovered);
            _isSelected = false;
        }
    }

    public bool IsSelected()
    {
        return _isSelected;
    }
}
