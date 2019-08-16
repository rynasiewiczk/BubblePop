using System;
using Model.CombiningBubbles.DroppingDisconnectedBubbles;
using Project.Grid;
using View.DroppingUnconnected;
using Zenject;

public class DroppingUnconnectedBubblesViewController : IDroppingUnconnectedBubblesViewController, IDisposable
{
    private readonly SignalBus _signalBus = null;
    private readonly DroppingBubbleViewPool _droppingBubbleViewPool = null;
    private readonly IGridMap _gridMap = null;

    private int _horizontalDirection = -1;

    public DroppingUnconnectedBubblesViewController(SignalBus signalBus, DroppingBubbleViewPool droppingBubbleViewPool, PiecesData piecesData, IGridMap gridMap)
    {
        _signalBus = signalBus;
        _droppingBubbleViewPool = droppingBubbleViewPool;
        _gridMap = gridMap;

        _signalBus.Subscribe<DroppingUnlinkedBubbleSignal>(DropBubbleView);
    }

    public void Dispose()
    {
        _signalBus.TryUnsubscribe<DroppingUnlinkedBubbleSignal>(DropBubbleView);
    }

    private void DropBubbleView(DroppingUnlinkedBubbleSignal signal)
    {
        var bubble = _droppingBubbleViewPool.Spawn();
        var position = _gridMap.GetViewPosition(signal.Position);

        bubble.Setup(position, signal.Level, _horizontalDirection);
        _horizontalDirection = _horizontalDirection == -1 ? 1 : -1;
    }
}