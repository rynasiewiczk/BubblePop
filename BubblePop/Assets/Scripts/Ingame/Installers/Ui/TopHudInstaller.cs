using UnityEngine;
using Zenject;

public class TopHudInstaller : MonoInstaller
{
    [SerializeField] private ProgressBarParticles _progressBarParticles = null;
    [SerializeField] private LevelProgressBarDisplay _levelProgressBarDisplay = null;
    [SerializeField] private RectTransform _particlesContainer = null;

    public override void InstallBindings()
    {
        Container.Bind<LevelProgressBarDisplay>().FromInstance(_levelProgressBarDisplay).AsSingle().NonLazy();
        Container.Bind<RectTransform>().FromInstance(_particlesContainer).AsSingle().NonLazy();

        Container.BindMemoryPool<ProgressBarParticles, ProgressBarParticlesPool>()
            .WithInitialSize(6)
            .FromComponentInNewPrefab(_progressBarParticles)
            .UnderTransform(_particlesContainer);
        Container.BindInterfacesTo<ProgressBarParticlesController>().AsSingle().NonLazy();
    }
}