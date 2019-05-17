using Model;
using Project.Aiming;
using Project.Bubbles;
using Zenject;

public class GridActionsInstaller : Installer<GridActionsInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<NextBubbleLevelToSpawnController>().AsSingle().NonLazy();
        
        Container.BindInterfacesTo<GameStateController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<StartAimingStateObserver>().AsSingle().NonLazy();
        Container.BindInterfacesTo<EndAimingStateObserver>().AsSingle().NonLazy();
    }
}