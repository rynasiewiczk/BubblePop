using UniRx;

namespace Project.Bubbles
{
    public interface IBubblesSpawner
    {
        ReactiveProperty<IBubble> JustSpawned { get; }
    }
}