using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    public interface IBubble
    {
        ReactiveProperty<Vector2Int> Position { get; }
        ReactiveProperty<int> Value { get; }
    }
}