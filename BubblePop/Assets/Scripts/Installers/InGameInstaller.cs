using UnityEngine;
using Zenject;

public class InGameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Random.InitState(42);
        
        GridActionsInstaller.Install(Container);
        GridInstaller.Install(Container);
    }
}