using UnityEngine;

namespace Toolbar
{
    public class MoveMode : ToolbarMode
    {
        private Camera _camera;
        
        [SerializeField] private Vector2 _dragAmount = Vector2.one;
        
        protected override void Awake()
        {
            _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            
            base.Awake();
        }
        
        public override bool SwipeScreen(Vector2 delta)
        {
            _camera.transform.localPosition += (Vector3) (delta * _dragAmount);
            return true;
        }
    }
}