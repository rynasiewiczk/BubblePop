using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PieceDataInstaller", menuName = "Installers/PieceDataInstaller")]
public class PieceDataInstaller : ScriptableObjectInstaller<PieceDataInstaller>
{
    [SerializeField] private PiecesData piecesData = null;

    public override void InstallBindings()
    {
        Container.Bind<PiecesData>().FromInstance(piecesData).AsSingle().NonLazy();
    }
}