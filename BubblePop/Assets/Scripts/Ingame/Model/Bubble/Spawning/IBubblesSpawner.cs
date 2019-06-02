using UniRx;

namespace Project.Pieces
{
    public interface IBubblesSpawner
    {
        ReactiveProperty<IPiece> LatestSpawnedBubble { get; }
        IPiece SpawnBubble(SpawnPieceOnGridSignal signal);
    }
}