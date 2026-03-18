using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private Sprite[] _frames;
    [SerializeField] private float _frameIncrement = 1.0f;
    private float _frameTimer;
    private int _currentFrame;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _frameTimer += Time.deltaTime;
        if(_frameTimer < _frameIncrement) { return; }
        _frameTimer -= _frameIncrement;

        _currentFrame = (_currentFrame + 1) % _frames.Length;
        _spriteRenderer.sprite = _frames[_currentFrame];
    }
}
