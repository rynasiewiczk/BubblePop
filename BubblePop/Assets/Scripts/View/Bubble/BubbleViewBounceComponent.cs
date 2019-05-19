using DG.Tweening;
using Project.Grid;
using UnityEngine;
using View;
using Zenject;

public class BubbleViewBounceComponent : MonoBehaviour
{
    [Inject] private readonly SignalBus _signalBus = null;
    [Inject] private readonly BubbleViewSettings _bubbleViewSettings = null;

    [SerializeField] private BubbleView _view = null;

    private Tween _tween = null;

    private void Awake()
    {
        Debug.Assert(_view, "Missing reference: _view", this);
    }

    private void Start()
    {
        _signalBus.Subscribe<OnBubbleHitSignal>(BounceOnHit);
    }

    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<OnBubbleHitSignal>(BounceOnHit);
    }

    private void BounceOnHit(OnBubbleHitSignal signal)
    {
        if (signal.Bubble != _view.Model)
        {
            return;
        }

        var direction = ((Vector2) transform.position - signal.SoucrePosition).normalized;
        _tween?.Kill();
        _tween = transform.DOLocalMove(direction * _bubbleViewSettings.OnHitBounceDistance, _bubbleViewSettings.OnHidBounceHalfDuration).OnComplete(() =>
        {
            _tween = transform.DOLocalMove(Vector2.zero, _bubbleViewSettings.OnHidBounceHalfDuration);
        });
    }
}