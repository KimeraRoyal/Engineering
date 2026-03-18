using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Board
{
    [Serializable]
    public class BoardPattern
    {
        private const int BitSize = sizeof(uint) * 8 - 1;
        private static readonly int ByteSize = MathUtil.BitWidth(BitSize + 1) - 1;

        [SerializeField] private List<uint> _pattern;
        [SerializeField] private Vector2Int _size;

        public Vector2Int Size => _size;

        public BoardPattern(List<uint> pattern, Vector2Int size)
        {
            _pattern = pattern;
            _size = size;
        }

        public BoardPattern(string definition)
        {
            _pattern = new List<uint>();
            ReadPattern(definition);
        }

        public void ReadPattern(string definition)
        {
            var lines = definition.Split('\n');

            _pattern.Clear();
            _size = new Vector2Int(0, lines.Length);
        
            foreach (var line in lines)
            {
                _size.x = Mathf.Max(_size.x, line.Length);
            }
        
            for(var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    if(lines[y][x] != '0') { SetTile(x + y * _size.x); }
                }
            }
        }

        public bool EvaluateTile(int index)
        {
            var segmentIndex = index >> ByteSize;
            if(segmentIndex >= _pattern.Count) { return false; }
            return (_pattern[segmentIndex] & (0x1u << (index & BitSize))) != 0;
        }

        public bool EvaluateTile(int x, int y)
            => EvaluateTile(x + y * _size.x);

        public bool EvaluateTile(Vector2Int tile)
            => EvaluateTile(tile.x + tile.y * _size.x);
    
        private void SetTile(int index)
        {
            var segmentIndex = index >> ByteSize;
            if(segmentIndex >= _pattern.Count) { SetCapacity(segmentIndex + 1); }
            _pattern[segmentIndex] |= 0x1u << (index & BitSize);
        }
    
        private void SetCapacity(int capacity)
        {
            if (_pattern.Count < capacity)
            {
                _pattern.AddRange(Enumerable.Repeat<uint>(0, capacity - _pattern.Count));
            }
            else if (_pattern.Count > capacity)
            {
                _pattern.RemoveRange(capacity, _pattern.Count - capacity);
            }
        }
    }
}