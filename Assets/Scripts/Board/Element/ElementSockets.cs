using System;
using UnityEngine;

namespace Board.Element
{
    public class ElementSockets : MonoBehaviour
    {
        public enum SocketType
        {
            In,
            Out,
            InOut
        }
    
        [Serializable]
        public class SocketInfo
        {
            [SerializeField] private SocketType _type;
        
            [SerializeField] private Vector2Int _position;

            public SocketType Type => _type;

            public Vector2Int Position => _position;
        }
    
        private Element _element;

        [SerializeField] private SocketInfo[] _info;
        [SerializeField] private Socket _socketPrefab;

        private Socket[] _sockets;

        private void Awake()
        {
            _element = GetComponentInParent<Element>();
            _element.OnFacingDirectionChanged.AddListener(AssignSockets);
        
            _sockets = new Socket[_info.Length];
        }

        private void Start()
        {
            AssignSockets(_element.Facing);
        }

        private void AssignSockets(Element.Direction facing)
        {
            for (var i = 0; i < _info.Length; i++)
            {
                var socket = GetOrCreateSocket(i);
                socket.Position = facing switch
                {
                    Element.Direction.Right =>  _info[i].Position,
                    Element.Direction.Down =>   new Vector2Int(                - _info[i].Position.y,                       _info[i].Position.x),
                    Element.Direction.Left =>   new Vector2Int(_element.Size.x - _info[i].Position.x - 1, _element.Size.y - _info[i].Position.y - 1),
                    Element.Direction.Up =>     new Vector2Int(_element.Size.x + _info[i].Position.y - 1, _element.Size.y - _info[i].Position.x - 1),
                    _ => throw new ArgumentOutOfRangeException(nameof(facing), facing, null)
                };
            }
        }

        private Socket GetOrCreateSocket(int index)
        {
            if (!_sockets[index])
            {
                _sockets[index] = Instantiate(_socketPrefab, transform);
            }
            return _sockets[index];
        }
    }
}
