using System;
using UnityEngine;
using UnityEngine.Events;

public class Toolbar : MonoBehaviour
{
    [Serializable]
    public class Mode
    {
        [SerializeField] private string _name = "Mode";
        
        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        public void Enter()
            => OnEnter?.Invoke();

        public void Exit()
            => OnExit?.Invoke();
    }

    [SerializeField] private Mode[] _modes;
    private int _currentMode = -1;

    private void Start()
    {
        ChangeMode(0);
    }

    public void ChangeMode(int mode)
    {
        if(mode == _currentMode || mode >= _modes.Length || mode < 0) { return; }
        
        if(_currentMode >= 0) { _modes[_currentMode].Exit(); }
        _currentMode = mode;
        _modes[_currentMode].Enter();
    }
}
