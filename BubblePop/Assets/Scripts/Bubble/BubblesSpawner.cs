using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    public class BubblesSpawner : IBubblesSpawner
    {
        private const int SAFETY_BUBBLE_SPAWN_VALUE = 4;

        public ReactiveProperty<IBubble> JustSpawned { get; private set; }

        private readonly BubblesPool _bubblesPool = null;
        private readonly BubbleData _bubbleData = null;

        public BubblesSpawner(BubblesPool bubblesPool, BubbleData bubbleData)
        {
            _bubblesPool = bubblesPool;
            _bubbleData = bubbleData;
        }

        private IBubble SpawnBubble(Vector2Int position, int value)
        {
            if (!Mathf.IsPowerOfTwo(value))
            {
                Debug.LogError("BubblesSpawner: value provided to spawn bubble is not a power of two! Provided value: " + value +
                               ". Returning bubble with safety value: " + SAFETY_BUBBLE_SPAWN_VALUE);
                SpawnBubble(position, SAFETY_BUBBLE_SPAWN_VALUE);
            }

            var bubble = _bubblesPool.Spawn(_bubbleData);
            bubble.Setup(position, value);

            return bubble;
        }
    }
}