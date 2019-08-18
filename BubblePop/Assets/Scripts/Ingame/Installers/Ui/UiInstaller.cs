using Ingame.View.UI.Popups.LevelUp;
using Project.Scripts.Popups;
using UnityEngine;
using View.GridScoresDisplay;
using Zenject;

public class UiInstaller : MonoInstaller
{
    [SerializeField] private GridScoreDisplayText _gridScoreDisplayText = null;

    public override void InstallBindings()
    {
        Container.BindMemoryPool<GridScoreDisplayText, GridScoreDisplayTextPool>()
            .WithInitialSize(6)
            .FromComponentInNewPrefab(_gridScoreDisplayText)
            .UnderTransformGroup("GridScoreTextsGroup");

        Container.DeclareSignal<PopupActionSignal>().OptionalSubscriber();
        Container.DeclareSignal<LevelUpPopupRequest>();
    }
}