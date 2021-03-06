using Project.Pieces;
using Zenject;
using UniRx;

namespace View
{
    public class BubbleViewPool : MonoMemoryPool<IPiece, GridBubble>
    {
        public BubbleViewPool(IBubblesSpawner bubblesSpawner)
        {
            bubblesSpawner.LatestSpawnedBubble.Where(x => x != null).Subscribe(x => Spawn(x));
        }

        protected override void Reinitialize(IPiece piece, GridBubble view)
        {
            view.Setup(piece);
            piece.Destroyed.Subscribe(x => Despawn(view));
        }
    }
}