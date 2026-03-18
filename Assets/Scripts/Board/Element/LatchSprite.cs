using UnityEngine;

namespace Board.Element
{
    public class LatchSprite : MonoBehaviour
    {
        private Element _element;
        private Latch _latch;

        private SpriteRenderer _spriteRenderer;

        [SerializeField] private Sprite[] _facingSprites;

        private void Awake()
        {
            _element = GetComponentInParent<Element>();
            _element.OnFacingDirectionChanged.AddListener(FacingChanged);
            
            _latch = GetComponentInParent<Latch>();
            _latch.OnValueChanged.AddListener(LatchChanged);

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FacingChanged(Element.Direction facing)
        {
            SetSprite(facing, _latch.Value);
        }

        private void LatchChanged(int amount)
        {
            SetSprite(_element.Facing, amount);
        }

        private void SetSprite(Element.Direction facing, int latchAmount)
        {
            _spriteRenderer.sprite = _facingSprites[(int)facing * _latch.ValueRange + latchAmount];
        }
    }
}