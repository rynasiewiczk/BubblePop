using Model.CombiningBubbles.DroppingDisconnectedBubbles;
using Project.Grid;
using UnityEngine;
using View.GridScoresDisplay;
using Zenject;

public class GridScoresDisplayController : MonoBehaviour
{
    [Inject] private readonly GridScoreDisplayTextPool _gridScoreDisplayTextPool = null;
    [Inject] private readonly SignalBus _signalBus = null;
    [Inject] private readonly BubbleData _bubbleData = null;
    [Inject] private readonly UiData _uiData = null;

    [Inject] private readonly IGridMap _gridMap = null;

    //[SerializeField] private Camera _camera = null;
    [SerializeField] private Transform _container = null;

    private void Awake()
    {
        Debug.Assert(_container, "Missing reference: _container", this);
    }

    private void Start()
    {
        _signalBus.Subscribe<BubblesCombiningDoneSignal>(ShowText);
        _signalBus.Subscribe<DroppingUnlinkedBubbleSignal>(ShowText);
    }

    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<BubblesCombiningDoneSignal>(ShowText);
        _signalBus.TryUnsubscribe<DroppingUnlinkedBubbleSignal>(ShowText);
    }

    private void ShowText(DroppingUnlinkedBubbleSignal signal)
    {
        ShowText(signal.Position, signal.Level);
    }

    private void ShowText(BubblesCombiningDoneSignal signal)
    {
        ShowText(signal.Position, signal.ResultLevel);
    }

    private void ShowText(Vector2 gridPosition, int level)
    {
        var text = _gridScoreDisplayTextPool.Spawn(_container);
        var score = _bubbleData.GetValueForLevel(level);
        var color = _bubbleData.GetColorForLevel(level);
        var fontSize = _uiData.GetFontSizeForScore(score);

        var position = new Vector2(gridPosition.x, _gridMap.GetHeightOfRows((int) gridPosition.y));
        text.Setup(position, score, fontSize, color);
    }
}