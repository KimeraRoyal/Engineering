using UnityEngine;

namespace Board.Element
{
    public class Socket : MonoBehaviour
    {
        private Vector2Int _position;

        public Vector2Int Position
        {
            get => _position;
            set
            {
                _position = value;
                transform.localPosition = new Vector3(_position.x, -_position.y);
            }
        }
    }
}