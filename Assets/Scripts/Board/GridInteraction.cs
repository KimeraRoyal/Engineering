using Input;
using UnityEngine;
using UnityEngine.Events;

namespace Board
{
    public class GridInteraction : MonoBehaviour
    {
        private Board _board;
        private MouseInput _mouse;
        private Camera _camera;

        private Vector2Int _hoveredTile;

        public bool IsPressed => _mouse.IsPointerPressed;

        public UnityEvent<Vector2Int> OnHoverTile;
        public UnityEvent<MouseButton, Vector2Int> OnPressTile;
        public UnityEvent<MouseButton, Vector2Int> OnUnpressTile;
    
        private void Awake()
        {
            _board = FindAnyObjectByType<Board>();
        
            _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        
            _mouse = FindAnyObjectByType<MouseInput>();
            _mouse.OnPress.AddListener(Press);
            _mouse.OnUnpress.AddListener(Unpress);
        }

        private void Update()
        {
            var mousePosition = UnityEngine.Input.mousePosition;
            mousePosition.z = transform.position.z;
            mousePosition = _camera.ScreenToWorldPoint(mousePosition) - _board.Offset;
        
            var currentHoveredTile = new Vector2Int(Mathf.FloorToInt(mousePosition.x), -Mathf.FloorToInt(mousePosition.y));
            if(currentHoveredTile == _hoveredTile) { return; }
            _hoveredTile = currentHoveredTile;
        
            OnHoverTile?.Invoke(_hoveredTile);
        }

        private void Press(MouseButton button, Vector2 mousePos)
        {
            OnPressTile?.Invoke(button, _hoveredTile);
        }

        private void Unpress(MouseButton button, Vector2 mousePos)
        {
            OnUnpressTile?.Invoke(button, _hoveredTile);
        }
    }
}
