using UniRx;

namespace Project.Bubbles
{
    public interface INextBubbleLevelToSpawnController
    {
        ReactiveProperty<int> BubbleLevelToSpawn { get; }
    }
}