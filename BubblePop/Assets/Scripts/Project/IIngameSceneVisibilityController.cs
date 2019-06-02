using Project;
using UniRx;

namespace OutGame
{
    public interface IIngameSceneVisibilityController
    {
        OnPlayerButtonClickDelegate OnPlayButtonClicked { get; }
        ReactiveCommand OnIngamePaused { get; }
        ReactiveCommand OnIngameUnpaused { get; }
    }
}