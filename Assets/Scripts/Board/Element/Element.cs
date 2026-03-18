using UnityEngine;
using UnityEngine.Events;

namespace Board.Element
{
    public class Element : MonoBehaviour
    {
        public enum Direction
        {
            Right,
            Down,
            Left,
            Up,
        }

        [SerializeField] private Vector2Int _size = Vector2Int.one;

        [SerializeField] private Direction _facing;
        [SerializeField] private bool _directional;
        [SerializeField] private bool _canRotate;

        [SerializeField] private bool _canSell;
        [SerializeField] private bool _canMove;

        public Vector2Int Size => _size;

        public Direction Facing
        {
            get => _facing;
            set
            {
                _facing = value;
                OnFacingDirectionChanged?.Invoke(_facing);
            }
        }
        public bool IsDirectional => _directional;
        public bool CanRotate => _canRotate;

        public bool CanSell
        {
            get => _canSell;
            set => _canSell = value;
        }

        public bool CanMove
        {
            get => _canMove;
            set => _canMove = value;
        }

        public UnityEvent<Direction> OnFacingDirectionChanged;

        public void Rotate(bool clockwise)
        {
            if(!_canRotate) { return; }
            _size = new Vector2Int(_size.y, _size.x);
            Facing = (Direction)(((int)_facing + (clockwise ? 1 : 3)) % (_directional ? 4 : 2));
        }

        public void Flip()
        {
            if(!_directional) { return; }
            Facing = (Direction)(((int)_facing + 2) % 4);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.A))
            {
                Rotate(false);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                Rotate(true);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.W))
            {
                Flip();
            }
        }
    }
}
