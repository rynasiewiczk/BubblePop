using Project.Grid;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GridSettingsInstaller", menuName = "Installers/GridSettingsInstaller")]
public class GridSettingsInstaller : ScriptableObjectInstaller<GridSettingsInstaller>
{
    [SerializeField] private GridSettings _gridSettings = null;

    public override void InstallBindings()
    {
        Container.Bind<GridSettings>().FromInstance(_gridSettings).AsSingle().NonLazy();
    }
}