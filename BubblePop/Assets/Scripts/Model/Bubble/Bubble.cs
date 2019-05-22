using System;
using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    [Serializable] public class Bubble : IBubble
    {
        public ReactiveProperty<Vector2Int> Position { get; private set; }
        public ReactiveProperty<int> Level { get; private set; }
        public ReactiveCommand<IBubble> Destroyed { get; private set; }

        public void Setup(Vector2Int position, int level)
        {
            Destroyed = new ReactiveCommand<IBubble>();

            Position = new ReactiveProperty<Vector2Int>(position);
            Level = new IntReactiveProperty(level);
        }

        public void Destroy()
        {
            if (Destroyed.IsDisposed)
            {
                Debug.LogWarning("Trying to destroy already destroyed bubble.");
            }

            Destroyed.Execute(this);
            Destroyed.Dispose();
        }

        public void MoveUpOneCell()
        {
            Position.Value = new Vector2Int(Position.Value.x, Position.Value.y + 1);
        }

        public void MoveDownOneCell()
        {
            Position.Value = new Vector2Int(Position.Value.x, Position.Value.y - 1);
        }
    }
}