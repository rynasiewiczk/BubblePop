using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "OutgameUiSettingsInstaller", menuName = "Installers/OutgameUiSettingsInstaller")]
public class OutgameUiSettingsInstaller : ScriptableObjectInstaller<OutgameUiSettingsInstaller>
{
    [SerializeField] private OutgameUiSettings _outgameUiSettings = null;

    public override void InstallBindings()
    {
        Container.Bind<OutgameUiSettings>().FromInstance(_outgameUiSettings).AsSingle().NonLazy();
    }
}