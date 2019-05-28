using System;
using Model.CombiningBubbles.DroppingDisconnectedBubbles;
using Project.Grid;
using View.DroppingUnconnected;
using Zenject;

public class DroppingUnconnectedBubblesViewController : IDroppingUnconnectedBubblesViewController, IDisposable
{
    private readonly SignalBus _signalBus = null;
    private readonly DroppingBubbleViewPool _droppingBubbleViewPool = null;
    private readonly PiecesData _piecesData = null;
    private readonly IGridMap _gridMap = null;

    private int _horizontalDirection = -1;

    public DroppingUnconnectedBubblesViewController(SignalBus signalBus, DroppingBubbleViewPool droppingBubbleViewPool, PiecesData piecesData, IGridMap gridMap)
    {
        _signalBus = signalBus;
        _droppingBubbleViewPool = droppingBubbleViewPool;
        _piecesData = piecesData;
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
        var color = _piecesData.GetColorForLevel(signal.Level);
        var value = _piecesData.GetValueInDisplayFormatFromPieceLevel(signal.Level, 0);

        bubble.Setup(position, color, value, _horizontalDirection);
        _horizontalDirection = _horizontalDirection == -1 ? 1 : -1;
    }
}