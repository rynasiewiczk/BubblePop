using DefaultNamespace;
using Model;
using Zenject;

public class GridActionsInstaller : Installer<GridActionsInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameStateController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<StartAimingStateObserver>().AsSingle().NonLazy();
        Container.BindInterfacesTo<EndAimingStateObserver>().AsSingle().NonLazy();
    }
}