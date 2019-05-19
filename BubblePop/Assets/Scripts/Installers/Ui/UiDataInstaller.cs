using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "UiDataInstaller", menuName = "Installers/UiDataInstaller")]
public class UiDataInstaller : ScriptableObjectInstaller<UiDataInstaller>
{
    [SerializeField] private UiData _uiData = null;

    public override void InstallBindings()
    {
        Container.Bind<UiData>().FromInstance(_uiData).AsSingle().NonLazy();
    }
}