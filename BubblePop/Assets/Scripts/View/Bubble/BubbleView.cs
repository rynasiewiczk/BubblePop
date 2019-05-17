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

    public IBubble Model = null;

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
        var bubbleSide = _gridMap.GetGridSideForRow(position.y);
        var offset = 0f;
        if (bubbleSide == GridRowSide.Right)
        {
            offset += .5f;
        }

        transform.position = (Vector2) position + new Vector2(offset, 0);
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

    private void Update() { }
}