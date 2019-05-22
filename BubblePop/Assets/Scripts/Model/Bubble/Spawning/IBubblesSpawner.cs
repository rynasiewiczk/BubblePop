using UniRx;

namespace Project.Bubbles
{
    public interface IBubblesSpawner
    {
        ReactiveProperty<IBubble> LatestSpawnedBubble { get; }
        IBubble SpawnBubble(SpawnBubbleOnGridSignal signal);
    }
}