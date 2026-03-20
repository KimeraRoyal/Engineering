using System;
using System.Linq;
using Input;
using TouchScript;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Pointers;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using Util;

public class MouseInput : MonoBehaviour
{
    [SerializeField] ScreenTransformGesture _swipeGesture;

    private int _pointerId = -1;
    [ReadOnly] [SerializeField] private Vector2 _pointerPosition;
    private MouseButton _pointerButton;
    
    public Vector2 PointerPosition => _pointerPosition;
    public bool IsPointerPressed => _pointerId >= 0;

    public UnityEvent<MouseButton, Vector2> OnPress;
    public UnityEvent<MouseButton, Vector2> OnUnpress;
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
        var pointer = e.Pointers[0];

        _pointerId = pointer.Id;
        _pointerPosition = pointer.Position;
        _pointerButton = GetMouseButton(pointer.Buttons);
        OnPress?.Invoke(_pointerButton, _pointerPosition);
    }

    private void PointersReleased(object sender, PointerEventArgs e)
    {
        var pointer = e.Pointers.FirstOrDefault(pointer => pointer.Id == _pointerId);
        if(pointer == null) { return; }
        
        _pointerPosition = pointer.Position;
        OnUnpress?.Invoke(_pointerButton, _pointerPosition);
        _pointerId = -1;
    }
    
    private void Swipe(object sender, EventArgs args)
    {
        _pointerPosition += new Vector2(_swipeGesture.DeltaPosition.x, _swipeGesture.DeltaPosition.y);
        OnSwipe?.Invoke(new Vector2(_swipeGesture.DeltaPosition.x / Screen.width, _swipeGesture.DeltaPosition.y / Screen.height));
    }

    private static MouseButton GetMouseButton(Pointer.PointerButtonState buttonState)
    {
        for (var i = 0; i < 3; i++)
        {
            if (CheckButton(buttonState, (Pointer.PointerButtonState)(1 << i))) { return (MouseButton)i; }
        }
        return MouseButton.Other;
    }

    private static bool CheckButton(Pointer.PointerButtonState buttonState, Pointer.PointerButtonState buttonPressed)
    {
        var increment = MathUtil.Log2((int)buttonPressed) * 2;
        return ((int)buttonState & ((int)buttonPressed | (1 << (11 + increment)) | (1 << (12 + increment)))) > 0;
    }
}