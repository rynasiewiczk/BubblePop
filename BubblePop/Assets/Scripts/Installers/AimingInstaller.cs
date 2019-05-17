using DefaultNamespace;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "AimingInstaller", menuName = "Installers/AimingInstaller")]
public class AimingInstaller : ScriptableObjectInstaller<AimingInstaller>
{
    [SerializeField] private AimingSettings _aimingSettings = null;

    public override void InstallBindings()
    {
        Container.Bind<AimingSettings>().FromInstance(_aimingSettings).AsSingle().NonLazy();
        Container.BindInterfacesTo<AimingDirectionObserver>().AsSingle().NonLazy();
        Container.BindInterfacesTo<BubbleDestinationFinder>().AsSingle().NonLazy();
    }
}