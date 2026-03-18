using System;
using System.Collections.Generic;
using Board;
using Board.Element;
using UnityEngine;

[CreateAssetMenu(fileName = "Board Layout", menuName = "Board Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]
    public class ElementInstance
    {
        [SerializeField] private Vector2Int _position;
        
        [SerializeField] private Element _elementPrefab;
        [SerializeField] private Element.Direction _facing;

        [SerializeField] private bool _canSell;
        [SerializeField] private bool _canMove;

        public Element SpawnElement(Transform parent)
        {
            var element = Instantiate(_elementPrefab, parent.position + new Vector3(_position.x, -_position.y), Quaternion.identity, parent);
            element.Facing = _facing;
            element.CanSell = _canSell;
            element.CanMove = _canMove;
            return element;
        }
    }
    
    [SerializeField] [TextArea(3, 10)] private string _layout;
    [SerializeField] private ElementInstance[] _elements;

    public BoardPattern CreatePattern()
        => new (_layout);

    public void SpawnElements(Transform parent, List<Element> elementList)
    {
        foreach (var element in _elements)
        {
            elementList.Add(element.SpawnElement(parent));
        }
    }
}
