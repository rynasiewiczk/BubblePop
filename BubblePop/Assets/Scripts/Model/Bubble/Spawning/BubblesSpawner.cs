using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Pieces
{
    public class BubblesSpawner : IBubblesSpawner, IDisposable
    {
        public ReactiveProperty<IBubble> LatestSpawnedBubble { get; private set; } = new ReactiveProperty<IBubble>();

        private readonly BubblesPool _bubblesPool = null;
        private readonly PiecesData _piecesData = null;
        private readonly SignalBus _signalBus = null;

        public BubblesSpawner(BubblesPool bubblesPool, PiecesData piecesData, SignalBus signalBus)
        {
            _bubblesPool = bubblesPool;
            _piecesData = piecesData;

            _signalBus = signalBus;
            _signalBus.Subscribe<SpawnPieceOnGridSignal>(signal => { SpawnBubble(signal); });
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SpawnPieceOnGridSignal>(signal => { SpawnBubble(signal); });
        }

        public IBubble SpawnBubble(SpawnPieceOnGridSignal signal)
        {
            return SpawnBubble(signal.Position, signal.Level);
        }

        private IBubble SpawnBubble(Vector2Int position, int level)
        {
            var bubble = _bubblesPool.Spawn(_piecesData);
            bubble.Setup(position, level);

            LatestSpawnedBubble.Value = bubble;

            return bubble;
        }
    }
}