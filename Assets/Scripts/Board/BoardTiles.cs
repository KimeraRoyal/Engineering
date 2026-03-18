using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Board
{
    [RequireComponent(typeof(Tilemap))]
    public class BoardTiles : MonoBehaviour
    {
        private Tilemap _tilemap;
        private Board _board;

        [SerializeField] private TileBase _boardTile;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
            
            _board = FindAnyObjectByType<Board>();
            _board.OnPatternChanged.AddListener(Generate);
        }

        public void Generate(BoardPattern pattern)
        {
            _tilemap.ClearAllTiles();

            var tileChanges = new List<TileChangeData>();
            for (var y = 0; y < pattern.Size.y; y ++)
            {
                for (var x = 0; x < pattern.Size.x; x ++)
                {
                    if(!pattern.EvaluateTile(x, y)) { continue; }
                    tileChanges.Add(new TileChangeData(new Vector3Int(x * 2,        -y * 2), _boardTile, Color.white, Matrix4x4.identity));
                    tileChanges.Add(new TileChangeData(new Vector3Int(x * 2 + 1,    -y * 2), _boardTile, Color.white, Matrix4x4.identity));
                    tileChanges.Add(new TileChangeData(new Vector3Int(x * 2,        -y * 2 - 1), _boardTile, Color.white, Matrix4x4.identity));
                    tileChanges.Add(new TileChangeData(new Vector3Int(x * 2 + 1,    -y * 2 - 1), _boardTile, Color.white, Matrix4x4.identity));
                }
            }
        
            _tilemap.SetTiles(tileChanges.ToArray(), false);
        }
    }
}