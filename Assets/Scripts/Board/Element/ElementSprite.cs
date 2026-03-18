using UnityEngine;

namespace Board.Element
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ElementSprite : MonoBehaviour
    {
        private Element _element;

        private SpriteRenderer _spriteRenderer;

        [SerializeField] private Sprite[] _facingSprites;

        private void Awake()
        {
            _element = GetComponentInParent<Element>();
            _element.OnFacingDirectionChanged.AddListener(FacingChanged);

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FacingChanged(Element.Direction facing)
        {
            _spriteRenderer.sprite = _facingSprites[(int)facing];
        }
    }
}
