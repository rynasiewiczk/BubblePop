using Ingame.Model.ExplodingAfterCombining;
using Model;
using Model.CombiningBubbles;
using Model.CombiningBubbles.DroppingDisconnectedBubbles;
using Model.FillingBubblesAbovePlayspace;
using Model.FindingMatches;
using Model.ScrollingRowsDown;
using Project.Aiming;
using Project.Pieces;
using Project.Pieces.PlacingOnGrid;
using View.GridScoresDisplay;
using Zenject;

public class GridActionsInstaller : Installer<GridActionsInstaller>
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<SpawnPieceOnGridSignal>();

        Container.BindInterfacesTo<SpawnShotBubbleOnGridController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<NextBubbleLevelToSpawnController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<GameStateController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<StartAimingStateObserver>().AsSingle().NonLazy();
        Container.BindInterfacesTo<FindingCellToShootPieceController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<BubbleFlyObserver>().AsSingle().NonLazy();
        Container.BindInterfacesTo<FindConnectedPiecesWithSameLevelController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<CombinePiecesController>().AsSingle().NonLazy();
        Container.DeclareSignal<CombinePieceSignal>();
        Container.DeclareSignal<PiecesCombiningDoneSignal>();

        Container.BindInterfacesTo<ExplodingOvergrownPiecesController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<DropUnlinkedBubblesController>().AsSingle().NonLazy();
        Container.DeclareSignal<DroppingUnlinkedBubbleSignal>();

        Container.BindInterfacesTo<ScrollRowsController>().AsSingle().NonLazy();

        Container.BindInterfacesTo<FillingBubblesAbovePlayspaceController>().AsSingle().NonLazy();
    }
}