using Board;
using Input;
using UnityEngine;

namespace Toolbar
{
    public class Toolbar : MonoBehaviour
    {
        private MouseInput _mouse;
        private GridInteraction _interaction;
        
        private ToolbarMode[] _modes;
        private int _currentMode = -1;

        private void Awake()
        {
            _modes = GetComponents<ToolbarMode>();
            
            _mouse = FindAnyObjectByType<MouseInput>();
            _interaction = FindAnyObjectByType<GridInteraction>();
        }

        private void OnEnable()
        {
            _mouse.OnPress.AddListener(PressScreen);
            _mouse.OnUnpress.AddListener(UnpressScreen);
            _mouse.OnSwipe.AddListener(SwipeScreen);
            
            _interaction.OnPressTile.AddListener(PressTile);
            _interaction.OnUnpressTile.AddListener(UnpressTile);
            _interaction.OnHoverTile.AddListener(HoverTile);
        }

        private void OnDisable()
        {
            _mouse.OnPress.RemoveListener(PressScreen);
            _mouse.OnUnpress.RemoveListener(UnpressScreen);
            _mouse.OnSwipe.RemoveListener(SwipeScreen);
            
            _interaction.OnPressTile.RemoveListener(PressTile);
            _interaction.OnUnpressTile.RemoveListener(UnpressTile);
            _interaction.OnHoverTile.RemoveListener(HoverTile);
        }

        private void Start()
        {
            ChangeMode(0);
        }

        public void ChangeMode(int mode)
        {
            if(mode == _currentMode || mode >= _modes.Length || mode < 0) { return; }
        
            if(_currentMode >= 0) { _modes[_currentMode].Exit(); }
            _currentMode = mode;
            _modes[_currentMode].Enter();
        }

        private void PressScreen(MouseButton button, Vector2 position)
            => _modes[_currentMode].PressScreen(button, position);

        private void UnpressScreen(MouseButton button, Vector2 position)
            => _modes[_currentMode].UnpressScreen(button, position);

        private void SwipeScreen(Vector2 delta)
        {
            if(_modes[_currentMode].SwipeScreen(delta)) { return; }
            _modes[0].SwipeScreen(delta);
        }

        private void PressTile(MouseButton button, Vector2Int tile)
            => _modes[_currentMode].PressTile(button, tile);

        private void UnpressTile(MouseButton button, Vector2Int tile)
            => _modes[_currentMode].UnpressTile(button, tile);

        private void HoverTile(Vector2Int tile)
            => _modes[_currentMode].HoverTile(tile);
    }

    [RequireComponent(typeof(Toolbar))]
    public abstract class ToolbarMode : MonoBehaviour
    {
        protected Toolbar Toolbar { get; private set; }

        protected virtual void Awake()
        {
            Toolbar = GetComponent<Toolbar>();
        }
        
        public virtual void Enter() {}
        public virtual void Exit() {}

        public virtual void PressScreen(MouseButton button, Vector2 position) { }
        public virtual void UnpressScreen(MouseButton button, Vector2 position) { }

        public virtual bool SwipeScreen(Vector2 delta) { return true; }

        public virtual void HoverTile(Vector2Int position) { }
        public virtual void PressTile(MouseButton button, Vector2Int position) { }
        public virtual void UnpressTile(MouseButton button, Vector2Int position) { }
    }
}