using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SelectionGraphic : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        private Board _board;
        private GridInteraction _interaction;

        private WirePlacer _wirePlacer;

        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _placingColor = Color.white;
        [SerializeField] private Color _destroyingColor = Color.white;

        [SerializeField] private bool _enabled;

        [SerializeField] private int _granularity = 1;
        private Vector2Int _currentPosition;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                var color = _spriteRenderer.color;
                color.a = 0.0f;
                _spriteRenderer.color = color;
            }
        }

        public int Granularity
        {
            get => _granularity;
            set
            {
                _granularity = value;
                _spriteRenderer.size = Vector2.one * (_granularity * 0.5f);
            }
        }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _board = FindAnyObjectByType<Board>();
            _interaction = FindAnyObjectByType<GridInteraction>();

            _wirePlacer = FindAnyObjectByType<WirePlacer>();
        
            _interaction.OnHoverTile.AddListener(Hover);
            
            _wirePlacer.OnBeginPlacing.AddListener(BeginPlacingWires);
            _wirePlacer.OnFinishPlacing.AddListener(FinishPlacingWires);
        }

        private void Start()
        {
            _spriteRenderer.size = Vector2.one * (_granularity * 0.5f);
            var color = _normalColor;
            color.a = 0.0f;
            _spriteRenderer.color = color;
        }

        private void BeginPlacingWires(bool creating)
        {
            var color = creating ? _placingColor : _destroyingColor;
            color.a = _enabled ? _spriteRenderer.color.a : 0.0f;
            _spriteRenderer.color = color;
        }

        private void FinishPlacingWires(bool creating)
        {
            var color = _normalColor;
            color.a = _enabled ? _spriteRenderer.color.a : 0.0f;
            _spriteRenderer.color = color;
        }

        private void Hover(Vector2Int position)
        {
            position /= _granularity;
            position *= _granularity;
            
            transform.localPosition = new Vector3(position.x, -position.y - (_granularity - 1), 0.0f);
            
            var color = _spriteRenderer.color;
            color.a = _enabled ? _board.IsWirePositionValid(position) ? 1.0f : 0.0f : 0.0f;
            _spriteRenderer.color = color;
        }
    }
}
