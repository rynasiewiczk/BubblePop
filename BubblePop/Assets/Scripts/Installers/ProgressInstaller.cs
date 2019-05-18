using Model.Progress.PlayerLevelController;
using Model.ScoreController;
using Zenject;

public class ProgressInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<ScoreController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PlayerLevelController>().AsSingle().NonLazy();
    }
}