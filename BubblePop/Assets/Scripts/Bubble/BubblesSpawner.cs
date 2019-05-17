using Project.Grid;
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
        private GridSettings _gridSettings = null;
        
        public BubblesSpawner(BubblesPool bubblesPool, BubbleData bubbleData, GridSettings gridSettings)
        {
            JustSpawned = new ReactiveProperty<IBubble>();
            
            _bubblesPool = bubblesPool;
            _bubbleData = bubbleData;
            _gridSettings = gridSettings;
        }

        public IBubble SpawnBubble(Vector2Int position, int level)
        {
            Debug.Log("Spawning bubble, level: " + level);
            var bubble = _bubblesPool.Spawn(_bubbleData);
            bubble.Setup(position, level, _gridSettings);

            JustSpawned.Value = bubble;
            
            return bubble;
        }
        
    }
}