using Model;
using Model.CombiningBubbles;
using Model.FindingMatches;
using Project.Aiming;
using Project.Bubbles;
using Project.Bubbles.PlacingOnGrid;
using Zenject;

public class GridActionsInstaller : Installer<GridActionsInstaller>
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<SpawnBubbleOnGridSignal>();

        Container.BindInterfacesTo<PlacerOfBubbleOnGrid>().AsSingle().NonLazy();
        Container.BindInterfacesTo<NextBubbleLevelToSpawnController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<GameStateController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<StartAimingStateObserver>().AsSingle().NonLazy();
        Container.BindInterfacesTo<EndAimingStateObserver>().AsSingle().NonLazy();

        Container.BindInterfacesTo<BubbleFlyObserver>().AsSingle().NonLazy();
        Container.BindInterfacesTo<FindConnectedBubblesWithSameLevelController>().AsSingle().NonLazy();
        
        Container.BindInterfacesTo<CombineBubbles>().AsSingle().NonLazy();
        Container.DeclareSignal<CombineBubbleSignal>();
    }
}