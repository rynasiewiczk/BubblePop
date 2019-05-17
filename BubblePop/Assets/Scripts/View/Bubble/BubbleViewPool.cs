using Project.Bubbles;
using Zenject;
using UniRx;
using UnityEngine;

namespace View
{
    public class BubbleViewPool : MonoMemoryPool<IBubble, BubbleView>
    {
        public BubbleViewPool(IBubblesSpawner bubblesSpawner)
        {
            bubblesSpawner.JustSpawned.Where(x => x != null).Subscribe(x => Spawn(x));
        }

        protected override void Reinitialize(IBubble bubble, BubbleView view)
        {
            view.Setup(bubble);
            bubble.Destroyed.Subscribe(x => Despawn(view));
        }
    }
}