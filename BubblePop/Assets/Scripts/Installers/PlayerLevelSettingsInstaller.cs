using Model.Progress.PlayerLevelController;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerLevelSettingsInstaller", menuName = "Installers/PlayerLevelSettingsInstaller")]
public class PlayerLevelSettingsInstaller : ScriptableObjectInstaller<PlayerLevelSettingsInstaller>
{
    [SerializeField] private PlayerLevelSettings _playerLevelSettings = null;
    
    public override void InstallBindings()
    {
        Container.Bind<PlayerLevelSettings>().FromInstance(_playerLevelSettings).AsSingle().NonLazy();
    }
}