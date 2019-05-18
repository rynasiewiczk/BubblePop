using System;
using Project.Grid;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Bubbles
{
    public class BubblesSpawner : IBubblesSpawner, IDisposable
    {
        public ReactiveProperty<IBubble> JustSpawned { get; private set; } = new ReactiveProperty<IBubble>();
        private readonly BubblesPool _bubblesPool = null;
        private readonly BubbleData _bubbleData = null;
        private readonly GridSettings _gridSettings = null;
        private SignalBus _signalBus = null;

        public BubblesSpawner(BubblesPool bubblesPool, BubbleData bubbleData, GridSettings gridSettings, SignalBus signalBus)
        {
            _bubblesPool = bubblesPool;
            _bubbleData = bubbleData;
            _gridSettings = gridSettings;

            _signalBus = signalBus;
            _signalBus.Subscribe<SpawnBubbleOnGridSignal>(signal => { SpawnBubble(signal); });
        }

        public void Dispose()
        {
            _signalBus.Subscribe<SpawnBubbleOnGridSignal>(signal => { SpawnBubble(signal); });
        }

        public IBubble SpawnBubble(SpawnBubbleOnGridSignal signal)
        {
            return SpawnBubble(signal.Position, signal.Level);
        }

        private IBubble SpawnBubble(Vector2Int position, int level)
        {
            var bubble = _bubblesPool.Spawn(_bubbleData);
            bubble.Setup(position, level, _gridSettings);

            JustSpawned.Value = bubble;

            return bubble;
        }
    }
}