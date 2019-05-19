using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    public interface IBubble
    {
        ReactiveProperty<Vector2Int> Position { get; }
        ReactiveProperty<int> Level { get; }

        void MoveUpOneCell();
        void MoveDown(int rows);
        
        bool IsPlayable();
        
        void Destroy();
        ReactiveCommand<Bubble> Destroyed { get; }
    }
}