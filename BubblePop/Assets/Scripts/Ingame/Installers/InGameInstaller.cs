using Model;
using UnityEngine;
using Zenject;

public class InGameInstaller : MonoInstaller
{
    [SerializeField] private GameplayCamera _gameplayCamera = null;
    
    public override void InstallBindings()
    {
        Random.InitState(42);
        
        Container.BindInterfacesTo<GameplayCamera>().FromInstance(_gameplayCamera).AsSingle();
        
        GridActionsInstaller.Install(Container);
        GridInstaller.Install(Container);
    }
}