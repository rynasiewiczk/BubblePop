using UniRx;

namespace Model.Progress.PlayerLevelController
{
    public interface IPlayerLevelController
    {
        ReactiveProperty<int> PlayerLevel { get; }
    }
}