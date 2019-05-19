using System;
using Model.CombiningBubbles.DroppingDisconnectedBubbles;
using Project.Grid;
using View.DroppingUnconnected;
using Zenject;

public class DroppingUnconnectedBubblesViewController : IDroppingUnconnectedBubblesViewController, IDisposable
{
    private readonly SignalBus _signalBus = null;
    private readonly DroppingBubbleViewPool _droppingBubbleViewPool = null;
    private readonly BubbleData _bubbleData = null;
    private readonly IGridMap _gridMap = null;


    public DroppingUnconnectedBubblesViewController(SignalBus signalBus, DroppingBubbleViewPool droppingBubbleViewPool, BubbleData bubbleData, IGridMap gridMap)
    {
        _signalBus = signalBus;
        _droppingBubbleViewPool = droppingBubbleViewPool;
        _bubbleData = bubbleData;
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
        var position = _gridMap.GetGridViewPosition(signal.Position);
        var color = _bubbleData.GetColorForLevel(signal.Level);
        var value = _bubbleData.GetValueForLevel(signal.Level);

        bubble.Setup(position, color, value);
    }
}