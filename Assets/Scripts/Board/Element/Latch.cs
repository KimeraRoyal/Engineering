using UnityEngine;
using UnityEngine.Events;

namespace Board.Element
{
    public class Latch : MonoBehaviour
    {
        [SerializeField] private int _value;
        [SerializeField] private int _valueRange = 2;

        public int Value => _value;
        public int ValueRange => _valueRange;

        public UnityEvent<int> OnValueChanged;

        public void Shift(bool up)
        {
            _value = (up ? 1 : _valueRange - 1) % _valueRange;
            OnValueChanged?.Invoke(_value);
        }

        public void Flip()
        {
            _value = _valueRange - _value - 1;
            OnValueChanged?.Invoke(_value);
        }
    }
}
