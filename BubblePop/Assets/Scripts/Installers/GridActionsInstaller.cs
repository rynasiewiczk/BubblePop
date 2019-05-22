using Model;
using Model.CombiningBubbles;
using Model.CombiningBubbles.DroppingDisconnectedBubbles;
using Model.FillingBubblesAbovePlayspace;
using Model.FindingMatches;
using Model.ScrollingRowsDown;
using Project.Aiming;
using Project.Bubbles;
using Project.Bubbles.PlacingOnGrid;
using View.GridScoresDisplay;
using Zenject;

public class GridActionsInstaller : Installer<GridActionsInstaller>
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<SpawnBubbleOnGridSignal>();

        Container.BindInterfacesTo<SpawnShotBubbleOnGridController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<NextBubbleLevelToSpawnController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<GameStateController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<StartAimingStateObserver>().AsSingle().NonLazy();
        Container.BindInterfacesTo<FindingCellToShootBubbleController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<BubbleFlyObserver>().AsSingle().NonLazy();
        Container.BindInterfacesTo<FindConnectedBubblesWithSameLevelController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<CombineBubblesController>().AsSingle().NonLazy();
        Container.DeclareSignal<CombineBubbleSignal>();
        Container.DeclareSignal<BubblesCombiningDoneSignal>();

        Container.BindInterfacesTo<DropUnlinkedBubblesController>().AsSingle().NonLazy();
        Container.DeclareSignal<DroppingUnlinkedBubbleSignal>();

        Container.BindInterfacesTo<ScrollRowsController>().AsSingle().NonLazy();
        Container.DeclareSignal<ScrollRowsSignal>();

        Container.BindInterfacesTo<FillingBubblesAbovePlayspaceController>().AsSingle().NonLazy();
    }
}