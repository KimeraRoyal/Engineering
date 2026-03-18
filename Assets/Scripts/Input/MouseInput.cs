using System;
using System.Linq;
using TouchScript;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Pointers;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MouseInput : MonoBehaviour
{
    [SerializeField] ScreenTransformGesture _swipeGesture;

    private int _pointerId = -1;
    [ReadOnly] [SerializeField] private Vector2 _pointerPosition;

    public Vector2 PointerPosition => _pointerPosition;
    public bool IsPointerPressed => _pointerId >= 0;

    public UnityEvent<Vector2> OnPress;
    public UnityEvent<Vector2> OnUnpress;
    public UnityEvent<Vector2> OnSwipe;

    void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed += PointersPressed;
            TouchManager.Instance.PointersReleased += PointersReleased;
        }
        _swipeGesture.Transformed += Swipe;
    }

    void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed -= PointersPressed;
            TouchManager.Instance.PointersReleased -= PointersReleased;
        }
        _swipeGesture.Transformed -= Swipe;
    }

    private void PointersPressed(object sender, PointerEventArgs e)
    {
        if(e.Pointers.Count != 1) { return; }

        _pointerId = e.Pointers[0].Id;
        _pointerPosition = e.Pointers[0].Position;
        OnPress?.Invoke(_pointerPosition);
    }

    private void PointersReleased(object sender, PointerEventArgs e)
    {
        Pointer pointer = e.Pointers.FirstOrDefault(pointer => pointer.Id == _pointerId);
        if(pointer == null) { return; }

        _pointerPosition = pointer.Position;
        OnUnpress?.Invoke(_pointerPosition);
        _pointerId = -1;
    }
    
    private void Swipe(object sender, EventArgs args)
    {
        _pointerPosition += new Vector2(_swipeGesture.DeltaPosition.x, _swipeGesture.DeltaPosition.y);
        OnSwipe?.Invoke(new Vector2(_swipeGesture.DeltaPosition.x / Screen.width, _swipeGesture.DeltaPosition.y / Screen.height));
    }
}
