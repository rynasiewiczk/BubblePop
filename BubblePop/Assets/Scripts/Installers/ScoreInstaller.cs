using Model.ScoreController;
using Zenject;

public class ScoreInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<ScoreController>().AsSingle().NonLazy();
    }
}