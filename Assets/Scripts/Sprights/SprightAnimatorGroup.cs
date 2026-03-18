using UnityEngine;

public class SprightAnimatorGroup : MonoBehaviour
{
    private SprightAnimator[] _animators;
    
    [SerializeField] private float _frameInterval = 1.0f;
    private float _frameTimer;
    private int _currentFrame;

    private void Awake()
    {
        _animators = GetComponentsInChildren<SprightAnimator>();
    }

    private void Update()
    {
        IncrementFrame();
    }
    
    private void IncrementFrame()
    {
        if(!IncrementTimer(ref _frameTimer, _frameInterval)) { return; }

        _currentFrame += 1;
        foreach (var animator in _animators)
        {
            animator.OnFrame(_currentFrame);
        }
    }

    private static bool IncrementTimer(ref float timer, float interval)
    {
        timer += Time.deltaTime;
        if(timer < interval) { return false; }
        timer -= interval;
        return true;
    }
}
