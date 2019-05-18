using Zenject;

public class SignalBusInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Zenject.SignalBusInstaller.Install(Container);
    }
}