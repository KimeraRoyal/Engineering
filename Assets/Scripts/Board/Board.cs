using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Board
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BoardLayout _layout;

        [SerializeField] private Vector2 _tileScale = Vector2.one;
        
        [SerializeField] private BoardPattern _currentPattern;
        [SerializeField] private Wires _wires;

        private List<Element.Element> _elements;

        public BoardPattern CurrentPattern => _currentPattern;
        public Wires Wires => _wires;

        public Vector2Int Size => _currentPattern.Size;
        public Vector3 Offset => transform.localPosition;

        public UnityEvent<BoardPattern> OnPatternChanged;

        public UnityEvent<Vector2Int, bool> OnWireChanged;

        private void Start()
        {
            SetPattern(_layout.CreatePattern());
            _elements = new List<Element.Element>();
            _layout.SpawnElements(transform, _elements);
        }

        public void SetPattern(BoardPattern pattern)
        {
            _currentPattern = pattern;
            _wires = new Wires(_currentPattern.Size * 2);
            
            transform.localPosition = new Vector3((int)(-pattern.Size.x * _tileScale.x), (int)(pattern.Size.y * _tileScale.y), 0.0f);
            
            OnPatternChanged?.Invoke(_currentPattern);
        }

        public bool GetWire(Vector2Int position)
            => IsWirePositionValid(position) && _wires.Get(position);

        public void SetWire(Vector2Int position)
        {
            if(!IsWirePositionValid(position) || _wires.Get(position)) { return; }
            _wires.Set(position);
            OnWireChanged?.Invoke(position, true);
        }

        public void ClearWire(Vector2Int position)
        {
            if(!IsWirePositionValid(position) || !_wires.Get(position)) { return; }
            _wires.Clear(position);
            OnWireChanged?.Invoke(position, false);
        }
        
        public void SetWireTo(Vector2Int position, bool value)
        {
            if(!IsWirePositionValid(position)) { return; }
            _wires.SetTo(position, value);
            OnWireChanged?.Invoke(position, value);
        }

        public void FlipWire(Vector2Int position)
        {
            if(!IsWirePositionValid(position)) { return; }
            OnWireChanged?.Invoke(position, _wires.Flip(position));
        }

        public bool IsWirePositionValid(Vector2Int position)
        {
            var boardPosition = new Vector2Int(position.x, position.y) / 2;
            return position is { x: >= 0, y: >= 0 } && position.x < _wires.Size.x && position.y < _wires.Size.y && _currentPattern.EvaluateTile(boardPosition);
        }
    }
}