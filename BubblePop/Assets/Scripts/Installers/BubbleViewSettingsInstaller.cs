using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "BubbleViewSettingsInstaller", menuName = "Installers/BubbleViewSettingsInstaller")]
public class BubbleViewSettingsInstaller : ScriptableObjectInstaller<BubbleViewSettingsInstaller>
{
    public override void InstallBindings()
    {
    }
}