using Project.Pieces;
using Project.Grid;
using Project.Grid.BuildingGrid;
using UnityEngine;
using Zenject;

public class GridInstaller : Installer<GridInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GridMap>().AsSingle().NonLazy();
        Container.BindInterfacesTo<BubblesSpawner>().AsSingle().NonLazy();

        Container.BindInterfacesTo<GridCellsBuilder>().AsSingle().NonLazy();
        Container.BindInterfacesTo<InitialPiecesBuilder>().AsSingle().NonLazy();

        Container.BindMemoryPool<Bubble, BubblesPool>()
            .WithInitialSize(70);
    }
}