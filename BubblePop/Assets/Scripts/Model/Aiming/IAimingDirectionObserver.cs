using UniRx;
using UnityEngine;

namespace Project.Aiming
{
    public interface IAimingDirectionObserver
    {
        ReactiveProperty<Vector2> AimingDirection { get; }
        Vector2 AimingStartPosition { get; }
    }
}