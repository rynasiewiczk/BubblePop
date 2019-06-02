using UnityEngine;
using Zenject;

public class OutgameInstaller : MonoInstaller
{
    [SerializeField] private PlayButtonController _playButtonController = null;

    public override void InstallBindings()
    {
        Container.Bind<PlayButtonController>().FromInstance(_playButtonController).AsSingle().NonLazy();
    }
}