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
    }

    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<BubblesCombiningDoneSignal>(ShowText);
    }

    private void ShowText(BubblesCombiningDoneSignal signal)
    {
        var text = _gridScoreDisplayTextPool.Spawn(_container);
        var score = _bubbleData.GetValueForLevel(signal.ResultLevel);
        var color = _bubbleData.GetColorForLevel(signal.ResultLevel);
        var fontSize = _uiData.GetFontSizeForScore(score);

        var position = new Vector2(signal.Position.x, _gridMap.GetHeightOfRows((int) signal.Position.y));
        text.Setup(position, score, fontSize, color);
    }
}