using UniRx;

namespace Project.Pieces
{
    public interface INextBubbleLevelToSpawnController
    {
        ReactiveProperty<int> NextBubbleLevelToSpawn { get; }
        ReactiveProperty<int>PreWarmedBubbleLevelToSpawn { get; }
        
        ReactiveCommand BubblesToSpawnUpdated { get; }
    }
}