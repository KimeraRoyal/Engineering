using System;
using TouchScript.Gestures;
using UnityEngine;

namespace Input
{
    [RequireComponent(typeof(PressGesture), typeof(Touchable))]
    public class PressTouchable : MonoBehaviour
    {
        [SerializeField] PressGesture _gesture;
        
        Touchable _touchable;

        void Awake()
        {
            if(!_gesture) { _gesture = GetComponent<PressGesture>(); }
            
            _touchable = GetComponent<Touchable>();

            _gesture.Pressed += Press;
        }

        void Press(object sender, EventArgs e)
        {
            _touchable.Touch();
        }
    }
}
