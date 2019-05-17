using System;
using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    [Serializable] public class Bubble : IBubble
    {
        public ReactiveProperty<Vector2Int> Position { get; private set; }
        public ReactiveProperty<int> Value { get; private set; }

        public void Setup(Vector2Int position, int value)
        {
            Position.Value = position;
            Value.Value = value;
        }
    }
}