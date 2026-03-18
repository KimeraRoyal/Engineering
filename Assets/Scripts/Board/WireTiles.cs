using UnityEngine;
using UnityEngine.Tilemaps;

namespace Board
{
    [RequireComponent(typeof(Tilemap))]
    public class WireTiles : MonoBehaviour
    {
        private Tilemap _tilemap;
        private Board _board;

        [SerializeField] private TileBase _wireTile;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
            
            _board = FindAnyObjectByType<Board>();
            _board.OnWireChanged.AddListener(UpdateWire);
        }

        private void UpdateWire(Vector2Int position, bool active)
        {
            _tilemap.SetTile(new Vector3Int(position.x, -position.y), active ? _wireTile : null);
        }
    }
}