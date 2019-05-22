using UniRx;

namespace Project.Pieces
{
    public interface IBubblesSpawner
    {
        ReactiveProperty<IBubble> LatestSpawnedBubble { get; }
        IBubble SpawnBubble(SpawnPieceOnGridSignal signal);
    }
}