using System;
using UnityEngine;
using View;
using Zenject;

public class BubbleViewBounceComponent : MonoBehaviour
{
    [Inject] private SignalBus _signalBus = null;

    [SerializeField] private BubbleView _view = null;

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
        
        
    }
}