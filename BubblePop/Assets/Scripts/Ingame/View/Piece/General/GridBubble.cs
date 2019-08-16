using System;
using Project.Pieces;
using Project.Grid;
using UnityEngine;
using View;
using Zenject;

public class GridBubble : MonoBehaviour
{
    [Inject] private readonly IGridMap _gridMap = null;

    [SerializeField] private PieceView _pieceView = null;

    public Action OnSetuped;

    public IPiece Model { get; private set; }

    private void Awake()
    {
        Debug.Assert(_pieceView, "Missing reference: _pieceView", this);
    }

    private void OnDisable()
    {
        Model = null;
    }

    public void Setup(IPiece model)
    {
        Model = model;
        SetPosition(model.Position.Value);
        SetView(model.Level.Value);

        OnSetuped?.Invoke();
    }

    private void SetPosition(Vector2Int position)
    {
        var viewPosition = _gridMap.GetViewPosition(position);
        transform.position = viewPosition;
    }

    private void SetView(int level)
    {
        _pieceView.Setup(level);
    }
}