using Project.Bubbles;
using Project.Grid;
using TMPro;
using UnityEngine;
using Zenject;

public class BubbleView : MonoBehaviour
{
    [Inject] private readonly BubbleData _bubbleData = null;
    [Inject] private readonly IGridMap _gridMap = null;

    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    [SerializeField] private TextMeshPro _text = null;

    public IBubble Model { get; private set; }

    private void Awake()
    {
        Debug.Assert(_spriteRenderer, "Missing reference: _spriteRenderer", this);
        Debug.Assert(_text, "Missing reference: _text", this);
    }

    public void Setup(IBubble model)
    {
        Model = model;
        SetPosition(model.Position.Value);
        SetColor(model.Level.Value);
        SetValue(model.Level.Value);
    }

    private void SetPosition(Vector2Int position)
    {
        var viewPosition = _gridMap.GetGridViewPosition(position);
        transform.position = viewPosition;
    }

    private void SetColor(int level)
    {
        var color = _bubbleData.GetColorForLevel(level);
        _spriteRenderer.color = color;
    }

    private void SetValue(int level)
    {
        _text.sortingOrder = 50;
        _text.text = _bubbleData.GetValueForLevel(level).ToString();
    }

    //private void Update() { }
}