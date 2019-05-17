using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IAimingDirectionObserver
    {
        ReactiveProperty<Vector2> AimingDirection { get; }
        Vector2 AimingStartPosition { get; }
    }
}