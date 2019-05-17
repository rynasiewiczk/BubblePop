using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IBubbleDestinationFinder
    {
        ReactiveProperty<Vector2Int> DestinationPosition { get; }
        
        Vector2? BubbleHitPoint { get; }
    }
}