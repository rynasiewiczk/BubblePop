using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    public interface IBubble
    {
        ReactiveProperty<Vector2Int> Position { get; }
        ReactiveProperty<int> Level { get; }

        void MoveUpOneCell();
        void MoveDownOneCell();
        
        void Destroy();
        ReactiveCommand<IBubble> Destroyed { get; }
    }
}