using System;
using UnityEngine;
using UnityEngine.Events;

namespace Board
{
    public class WirePlacer : MonoBehaviour
    {
        private Board _board;
    
        private GridInteraction _interaction;

        private bool _holding;
        private bool _placing;

        public UnityEvent<bool> OnBeginPlacing;
        public UnityEvent<bool> OnFinishPlacing;

        private void Awake()
        {
            _board = FindAnyObjectByType<Board>();
        
            _interaction = FindAnyObjectByType<GridInteraction>();
        }

        private void OnEnable()
        {
            _interaction.OnPressTile.AddListener(Press);
            _interaction.OnUnpressTile.AddListener(Unpress);
            _interaction.OnHoverTile.AddListener(Hover);
        }

        private void OnDisable()
        {
            _interaction.OnPressTile.RemoveListener(Press);
            _interaction.OnUnpressTile.RemoveListener(Unpress);
            _interaction.OnHoverTile.RemoveListener(Hover);
        }

        private void Press(Vector2Int tile)
        {
            if(!_board.IsWirePositionValid(tile)) { return; }
            _board.FlipWire(tile);
            _holding = true;
            _placing = _board.GetWire(tile);
            OnBeginPlacing?.Invoke(_placing);
        }

        private void Unpress(Vector2Int tile)
        {
            if(!_holding) { return; }
            _holding = false;
            OnFinishPlacing?.Invoke(_placing);
        }

        private void Hover(Vector2Int tile)
        {
            if(!_holding) { return; }
            _board.SetWireTo(tile, _placing);
        }
    }
}
