using System;
using Board;
using Input;
using UnityEngine;

namespace Toolbar
{
    public class WireMode : ToolbarMode
    {
        private enum PlaceMode
        {
            None,
            Placing,
            Destroying,
            Moving
        }
        
        private Board.Board _board;
        private SelectionGraphic _selection;
        
        private PlaceMode _placeMode;

        protected override void Awake()
        {
            base.Awake();
            
            _board = FindAnyObjectByType<Board.Board>();
            _selection = FindAnyObjectByType<SelectionGraphic>();
        }

        public override void Enter()
        {
            _selection.SelectionGranularity = Vector2Int.one;
            _selection.SelectionSize = Vector2Int.one;
        }

        public override void Exit()
        {
            _placeMode = PlaceMode.None;
            _selection.ChangeMode(SelectionGraphic.SelectionMode.Disabled);
        }

        public override bool SwipeScreen(Vector2 delta)
        {
            return _placeMode != PlaceMode.Moving;
        }

        public override void HoverTile(Vector2Int position)
        {
            UpdateSelection(position);
            _selection.Hover(position);
            
            if(_placeMode is PlaceMode.None or PlaceMode.Moving) { return; }
            _board.SetWireTo(position, _placeMode == PlaceMode.Placing);
        }

        public override void PressTile(MouseButton button, Vector2Int position)
        {
            if (!_board.IsWirePositionValid(position))
            {
                _placeMode = PlaceMode.Moving;
                UpdateSelection(position);
                return;
            }

            _placeMode = button switch
            {
                MouseButton.LMB => PlaceMode.Placing,
                MouseButton.RMB => PlaceMode.Destroying,
                MouseButton.MMB => PlaceMode.Moving,
                MouseButton.Other => PlaceMode.None,
                _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
            };
            _board.SetWireTo(position, _placeMode == PlaceMode.Placing);
            UpdateSelection(position);
        }

        public override void UnpressTile(MouseButton button, Vector2Int position)
        {
            _placeMode = PlaceMode.None;
            UpdateSelection(position);
        }
        
        private void UpdateSelection(Vector2Int position)
        {
            var mode = SelectionGraphic.SelectionMode.Disabled;
            if (_board.IsWirePositionValid(position))
            {
                mode = _placeMode switch
                {
                    PlaceMode.None => SelectionGraphic.SelectionMode.Selecting,
                    PlaceMode.Placing => SelectionGraphic.SelectionMode.Placing,
                    PlaceMode.Destroying => SelectionGraphic.SelectionMode.Destroying,
                    PlaceMode.Moving => SelectionGraphic.SelectionMode.Disabled,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            _selection.ChangeMode(mode);
        }
    }
}