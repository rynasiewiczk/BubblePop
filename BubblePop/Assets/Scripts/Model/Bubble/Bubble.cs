using System;
using Project.Grid;
using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    [Serializable] public class Bubble : IBubble
    {
        private GridSettings _gridSettings;

        public ReactiveProperty<Vector2Int> Position { get; private set; }
        public ReactiveProperty<int> Level { get; private set; }
        public ReactiveCommand<Bubble> Destroyed { get; private set; }

        public void Setup(Vector2Int position, int level, GridSettings gridSettings)
        {
            _gridSettings = gridSettings;

            Destroyed = new ReactiveCommand<Bubble>();

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

        public void MoveDown(int rows)
        {
            Position.Value = new Vector2Int(Position.Value.x, Position.Value.y - rows);
        }

        public bool IsPlayable()
        {
            return !Destroyed.IsDisposed && Position.Value.y < _gridSettings.StartGridSize.y;
        }
    }
}