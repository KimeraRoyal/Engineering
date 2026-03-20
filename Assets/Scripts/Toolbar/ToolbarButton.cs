using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToolbarButton : MonoBehaviour
{
    private Toolbar.Toolbar _toolbar;
    private Button _button;
    
    [SerializeField] private int _mode = -1;

    private void Awake()
    {
        _toolbar = FindAnyObjectByType<Toolbar.Toolbar>();
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        _toolbar.ChangeMode(_mode);
    }
}
