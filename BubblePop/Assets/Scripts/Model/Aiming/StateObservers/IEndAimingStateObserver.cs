using UniRx;
using UnityEngine;

namespace Project.Aiming
{
    public interface IEndAimingStateObserver
    {
        ReactiveProperty<Vector2[]> BubbleFlyPath { get; }
        Vector2Int BubbleDestination { get; }
    }
}