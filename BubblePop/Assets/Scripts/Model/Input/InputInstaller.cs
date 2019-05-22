using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera = null;
    
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        Container.BindInterfacesTo<MouseInputEventsNotifier>().AsSingle().NonLazy();
    }
}