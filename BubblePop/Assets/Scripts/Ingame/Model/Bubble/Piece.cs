using System;
using UniRx;
using UnityEngine;

namespace Project.Pieces
{
    [Serializable] public class Piece : IPiece
    {
        public ReactiveProperty<Vector2Int> Position { get; private set; }
        public ReactiveProperty<int> Level { get; private set; }
        public ReactiveCommand<IPiece> Destroyed { get; private set; }

        public void Setup(Vector2Int position, int level)
        {
            Destroyed = new ReactiveCommand<IPiece>();

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