using UnityEngine;
using View;
using Zenject;

[CreateAssetMenu(fileName = "BubbleViewSettingsInstaller", menuName = "Installers/BubbleViewSettingsInstaller")]
public class BubbleViewSettingsInstaller : ScriptableObjectInstaller<BubbleViewSettingsInstaller>
{
    [SerializeField] private BubbleViewSettings _bubbleViewSettings = null;

    public override void InstallBindings()
    {
        Container.Bind<BubbleViewSettings>().FromInstance(_bubbleViewSettings).AsSingle().NonLazy();
    }
}