using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "BubbleDataInstaller", menuName = "Installers/BubbleDataInstaller")]
public class BubbleDataInstaller : ScriptableObjectInstaller<BubbleDataInstaller>
{
    [SerializeField] private BubbleData _bubbleData;

    public override void InstallBindings()
    {
        Container.Bind<BubbleData>().FromInstance(_bubbleData).AsSingle().NonLazy();
    }
}