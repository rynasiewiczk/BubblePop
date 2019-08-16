using Ingame.Model.ExplodingAfterCombining;
using Model;
using View.FlyingAfterAiming;
using Zenject;

public class SignalBusInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Zenject.SignalBusInstaller.Install(Container);

        Container.DeclareSignal<IngamePausedSignal>();
        Container.DeclareSignal<BubbleFlySignal>();
        Container.DeclareSignal<OvergrownExplosionSignal>();
    }
}