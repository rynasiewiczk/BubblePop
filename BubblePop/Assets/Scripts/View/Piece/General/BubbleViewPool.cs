using Project.Pieces;
using Zenject;
using UniRx;
using UnityEngine;

namespace View
{
    public class BubbleViewPool : MonoMemoryPool<IPiece, BubbleView>
    {
        public BubbleViewPool(IBubblesSpawner bubblesSpawner)
        {
            bubblesSpawner.LatestSpawnedBubble.Where(x => x != null).Subscribe(x => Spawn(x));
        }

        protected override void Reinitialize(IPiece piece, BubbleView view)
        {
            view.Setup(piece);
            piece.Destroyed.Subscribe(x => Despawn(view));
        }
    }
}