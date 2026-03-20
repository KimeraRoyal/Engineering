using System;
using UnityEngine;
using Util;

namespace Board
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SelectionGraphic : MonoBehaviour
    {
        public enum SelectionMode
        {
            Disabled,
            Selecting,
            Placing,
            Destroying
        }
        
        private SpriteRenderer _spriteRenderer;
        
        private SelectionMode _mode;

        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _placingColor = Color.white;
        [SerializeField] private Color _destroyingColor = Color.white;

        [SerializeField] private Vector2Int _selectionGranularity = Vector2Int.one;
        [SerializeField] private Vector2Int _selectionSize = Vector2Int.one;
        private Vector2Int _currentPosition;

        public bool Enabled => _mode != SelectionMode.Disabled;

        public Vector2Int SelectionGranularity
        {
            get => _selectionGranularity;
            set
            {
                _selectionGranularity = value;           
                transform.localPosition = GetAlignedPosition();
            }
        }

        public Vector2Int SelectionSize
        {
            get => _selectionSize;
            set
            {
                _selectionSize = value;
                _spriteRenderer.size = (Vector2)_selectionSize * 0.5f;
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _spriteRenderer.size = (Vector2)_selectionSize * 0.5f;
            var color = _normalColor;
            color.a = 0.0f;
            _spriteRenderer.color = color;
        }

        public void ChangeMode(SelectionMode mode)
        {
            if(mode == _mode) { return; }
            _mode = mode;
            
            var color = mode switch
            {
                SelectionMode.Disabled => Color.clear,
                SelectionMode.Selecting => _normalColor,
                SelectionMode.Placing => _placingColor,
                SelectionMode.Destroying => _destroyingColor,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            color.a = Enabled ? color.a : 0.0f;
            Debug.Log(color.a);
            _spriteRenderer.color = color;
        }

        public void Hover(Vector2Int position)
        {
            _currentPosition = position;
            transform.localPosition = GetAlignedPosition();
        }

        private Vector2 GetAlignedPosition()
        {
            var position = _currentPosition;
            for (var axis = 0; axis < 2; axis++)
            {
                position[axis] /= _selectionGranularity[axis];
                position[axis] *= _selectionGranularity[axis];
            }
            return new Vector2(position.x, -position.y + 1);
        }
    }
}
