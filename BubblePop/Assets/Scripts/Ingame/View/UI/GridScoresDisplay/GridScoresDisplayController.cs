using Model.CombiningBubbles.DroppingDisconnectedBubbles;
using Project.Grid;
using UnityEngine;
using View.GridScoresDisplay;
using Zenject;

public class GridScoresDisplayController : MonoBehaviour
{
    private const float POSITION_UP_OFFSET = .75f;

    [Inject] private readonly GridScoreDisplayTextPool _gridScoreDisplayTextPool = null;
    [Inject] private readonly SignalBus _signalBus = null;
    [Inject] private readonly PiecesData _piecesData = null;
    [Inject] private readonly UiData _uiData = null;

    [Inject] private readonly IGridMap _gridMap = null;

    [SerializeField] private Transform _container = null;

    private void Awake()
    {
        Debug.Assert(_container, "Missing reference: _container", this);
    }

    private void Start()
    {
        _signalBus.Subscribe<PiecesCombiningDoneSignal>(ShowText);
        _signalBus.Subscribe<DroppingUnlinkedBubbleSignal>(ShowText);
    }

    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<PiecesCombiningDoneSignal>(ShowText);
        _signalBus.TryUnsubscribe<DroppingUnlinkedBubbleSignal>(ShowText);
    }

    private void ShowText(DroppingUnlinkedBubbleSignal signal)
    {
        ShowText(signal.Position, signal.Level);
    }

    private void ShowText(PiecesCombiningDoneSignal signal)
    {
        ShowText(signal.Position, signal.ResultLevel);
    }

    private void ShowText(Vector2 gridPosition, int level)
    {
        var text = _gridScoreDisplayTextPool.Spawn(_container);
        var score = _piecesData.GetValueForLevel(level);
        var fontSize = _uiData.GetFontSizeForScore(score);
        var scoreInFormatToDisplay = _piecesData.GetValueInDisplayFormat(score, 0);
        var color = _piecesData.GetColorsSetForLevel(level).InnerColor;

        var displayPosition = GetDisplayPoint(gridPosition);

        text.Setup(displayPosition, scoreInFormatToDisplay, fontSize, color);
    }

    private Vector2 GetDisplayPoint(Vector2 gridPosition)
    {
        var position = _gridMap.GetViewPosition(gridPosition);
        position = new Vector2(position.x, position.y + POSITION_UP_OFFSET);
        return position;
    }
}