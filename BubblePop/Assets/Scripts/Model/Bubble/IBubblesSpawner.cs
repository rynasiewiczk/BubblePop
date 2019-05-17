using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    public interface IBubblesSpawner
    {
        ReactiveProperty<IBubble> JustSpawned { get; }

        IBubble SpawnBubble(Vector2Int position, int level);
    }
}