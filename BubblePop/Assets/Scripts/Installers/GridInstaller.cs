using Project.Bubbles;
using Project.Grid;
using Project.Grid.BuildingGrid;
using Zenject;

public class GridInstaller : Installer<GridInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GridMap>().AsSingle().NonLazy();
        Container.BindInterfacesTo<GridCellsBuilder>().AsSingle().NonLazy();

        Container.BindInterfacesTo<BubblesSpawner>().AsSingle().NonLazy();

        Container.BindMemoryPool<Bubble, BubblesPool>()
            .WithInitialSize(54);
    }
}