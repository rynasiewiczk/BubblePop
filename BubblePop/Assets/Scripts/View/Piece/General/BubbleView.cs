using System;
using Project.Pieces;
using Project.Grid;
using TMPro;
using UnityEngine;
using Zenject;

public class BubbleView : MonoBehaviour
{
    [Inject] private readonly PiecesData _piecesData = null;
    [Inject] private readonly IGridMap _gridMap = null;

    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    [SerializeField] private TextMeshPro _text = null;

    public Action OnSetuped;

    public IPiece Model { get; private set; }

    private void Awake()
    {
        Debug.Assert(_spriteRenderer, "Missing reference: _spriteRenderer", this);
        Debug.Assert(_text, "Missing reference: _text", this);
    }

    private void OnDisable()
    {
        Model = null;
    }

    public void Setup(IPiece model)
    {
        Model = model;
        SetPosition(model.Position.Value);
        SetColor(model.Level.Value);
        SetValue(model.Level.Value);

        OnSetuped?.Invoke();
    }

    private void SetPosition(Vector2Int position)
    {
        var viewPosition = _gridMap.GetViewPosition(position);
        transform.position = viewPosition;
    }

    private void SetColor(int level)
    {
        var color = _piecesData.GetColorForLevel(level);
        _spriteRenderer.color = color;
    }

    private void SetValue(int level)
    {
        _text.sortingOrder = 50;
        _text.text = _piecesData.GetValueInDisplayFormatFromPieceLevel(level, 0);
    }
}