using Enums;
using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IBubbleDestinationFinder
    {
        ReactiveProperty<Vector2Int> DestinationPosition { get; }
        //BubbleSide BubbleHitSide { get; }
        BubbleAimedData BubbleAimedData { get; }
    }
}