using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class SprightAnimator : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Vector3 _offsetMax = Vector2.one;
    [SerializeField] private int _minOffsetFrameDuration = 1, _maxOffsetFrameDuration = 5;
    [SerializeField] private float _offsetSlerpSpeedMin = 0.1f, _offsetSlerpSpeedMax = 0.2f;
    [SerializeField] private float _offsetLerpSpeedMin = 0.1f, _offsetLerpSpeedMax = 0.2f;
    private Vector2 _offset;
    private int _offsetFrameDuration;
    private float _offsetSlerpSpeed;
    private float _offsetLerpSpeed;

    [SerializeField] private Sprite[] _frames;
    [SerializeField] private int _minFrameDuration = 1, _maxFrameDuration = 5;
    private int _frameIndex;
    private int _frameDuration;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _offsetFrameDuration = Random.Range(_minOffsetFrameDuration, _maxOffsetFrameDuration);
        _frameDuration = Random.Range(_minFrameDuration, _maxFrameDuration);
        _offsetSlerpSpeed = Random.Range(_offsetSlerpSpeedMin, _offsetSlerpSpeedMax);
        _offsetLerpSpeed = Random.Range(_offsetLerpSpeedMin, _offsetLerpSpeedMax);
    }

    private void Update()
    {
        transform.localPosition = Vector3.Slerp(Vector3.Lerp(transform.localPosition, _offset, _offsetLerpSpeed), _offset, _offsetSlerpSpeed);
    }
    
    public void OnFrame(int frame)
    {
        ChangeOffset(frame);
        Animate(frame);
    }

    private void ChangeOffset(int frame)
    {
        if(frame % _offsetFrameDuration != 0) { return; }
        _offset = new Vector2(Random.Range(-_offsetMax.x, _offsetMax.x), Random.Range(-_offsetMax.y, _offsetMax.y));
    }

    private void Animate(int frame)
    {
        if(frame % _frameDuration != 0) { return; }
        _frameIndex = (_frameIndex + 1) % _frames.Length;
        _spriteRenderer.sprite = _frames[_frameIndex];
    }
}
