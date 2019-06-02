using Model;
using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<MouseInputEventsNotifier>().AsSingle().NonLazy();
    }
}