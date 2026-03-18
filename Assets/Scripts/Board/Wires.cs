using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Board
{
    [Serializable]
    public class Wires
    {
        private const int BitSize = sizeof(uint) * 8 - 1;
        private static readonly int ByteSize = MathUtil.BitWidth(BitSize + 1) - 1;

        [SerializeField] private List<uint> _wires;
        [SerializeField] private Vector2Int _size;

        public Vector2Int Size => _size;
        
        public Wires(Vector2Int size)
        {
            _wires = new List<uint>();
            _size = size;

            var capacity = (_size.x * _size.y) >> ByteSize;
            SetCapacity(capacity);
        }

        public bool Get(int index)
        {
            var segmentIndex = index >> ByteSize;
            if(segmentIndex >= _wires.Count) { return false; }
            return (_wires[segmentIndex] & (0x1u << (index & BitSize))) != 0;
        }

        public bool Get(int x, int y)
            => Get(x + y * _size.x);

        public bool Get(Vector2Int position)
            => Get(position.x + position.y * _size.x);

        public bool SetTo(int index, bool value)
        {
            if (value)
            {
                Set(index);
            }
            else
            {
                Clear(index);
            }
            return value;
        }

        public bool SetTo(int x, int y, bool value)
            => SetTo(x + y * _size.x, value);

        public bool SetTo(Vector2Int position, bool value)
            => SetTo(position.x + position.y * _size.x, value);

        public void Set(int index)
        {
            var segmentIndex = index >> ByteSize;
            if(segmentIndex >= _wires.Count) { SetCapacity(segmentIndex + 1); }
            _wires[segmentIndex] |= 0x1u << (index & BitSize);
        }

        public void Set(int x, int y)
            => Set(x + y * _size.x);

        public void Set(Vector2Int position)
            => Set(position.x + position.y * _size.x);

        public void Clear(int index)
        {
            var segmentIndex = index >> ByteSize;
            if(segmentIndex >= _wires.Count) { return; }
            _wires[segmentIndex] &= ~(0x1u << (index & BitSize));
        }

        public void Clear(int x, int y)
            => Clear(x + y * _size.x);

        public void Clear(Vector2Int position)
            => Clear(position.x + position.y * _size.x);

        public bool Flip(int index)
            => SetTo(index, !Get(index));

        public bool Flip(int x, int y)
            => Flip(x + y * _size.x);

        public bool Flip(Vector2Int position)
            => Flip(position.x + position.y * _size.x);
    
        private void SetCapacity(int capacity)
        {
            if (_wires.Count < capacity)
            {
                _wires.AddRange(Enumerable.Repeat<uint>(0, capacity - _wires.Count));
            }
            else if (_wires.Count > capacity)
            {
                _wires.RemoveRange(capacity, _wires.Count - capacity);
            }
        }
    }
}