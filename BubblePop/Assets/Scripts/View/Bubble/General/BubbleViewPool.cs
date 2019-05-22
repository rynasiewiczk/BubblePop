using Project.Pieces;
using Zenject;
using UniRx;
using UnityEngine;

namespace View
{
    public class BubbleViewPool : MonoMemoryPool<IBubble, BubbleView>
    {
        public BubbleViewPool(IBubblesSpawner bubblesSpawner)
        {
            bubblesSpawner.LatestSpawnedBubble.Where(x => x != null).Subscribe(x => Spawn(x));
        }

        protected override void Reinitialize(IBubble bubble, BubbleView view)
        {
            view.Setup(bubble);
            bubble.Destroyed.Subscribe(x => Despawn(view));
        }
    }
}