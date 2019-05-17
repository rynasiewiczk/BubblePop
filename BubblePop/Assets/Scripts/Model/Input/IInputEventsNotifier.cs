using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IInputEventsNotifier
    {
        ReactiveProperty<Vector2> OnInputStart { get; }
        ReactiveProperty<Vector2> OnInputEnd { get; }
        
        ReactiveProperty<Vector2> OnInputMove { get; }
    }
}