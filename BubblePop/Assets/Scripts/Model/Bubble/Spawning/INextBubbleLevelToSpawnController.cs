using UniRx;

namespace Project.Bubbles
{
    public interface INextBubbleLevelToSpawnController
    {
        ReactiveProperty<int> NextBubbleLevelToSpawn { get; }
        ReactiveProperty<int>PreWarmedBubbleLevelToSpawn { get; }
        
        ReactiveCommand BubblesToSpawnUpdated { get; }
    }
}