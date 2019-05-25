using UnityEngine;
using Zenject;

namespace Project.Aiming
{
    [CreateAssetMenu(fileName = "AimingInstaller", menuName = "Installers/AimingInstaller")]
    public class AimingInstaller : ScriptableObjectInstaller<AimingInstaller>
    {
        [SerializeField] private AimingSettings _aimingSettings = null;

        public override void InstallBindings()
        {
            Container.Bind<AimingSettings>().FromInstance(_aimingSettings).AsSingle().NonLazy();
            Container.BindInterfacesTo<AimingStartPointProvider>().AsSingle().NonLazy();
            
            Container.BindInterfacesTo<AimingDirectionObserver>().AsSingle().NonLazy();
            Container.BindInterfacesTo<AimEndPointFinder>().AsSingle().NonLazy();
        }
    }
}