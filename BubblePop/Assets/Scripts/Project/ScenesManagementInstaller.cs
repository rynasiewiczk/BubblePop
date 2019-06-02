using UnityEngine;
using Zenject;

public class ScenesManagementInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<IngameSceneVisibilityController>().AsSingle().NonLazy();
    }
}