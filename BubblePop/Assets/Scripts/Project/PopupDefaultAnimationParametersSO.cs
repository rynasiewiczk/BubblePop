using Project.Scripts.Popups.AnimationTransitions;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PopupDefaultAnimationParametersSO", menuName = "Installers/PopupDefaultAnimationParametersSO")]
public class PopupDefaultAnimationParametersSO : ScriptableObjectInstaller<PopupDefaultAnimationParametersSO>
{
    [SerializeField] private PopupAnimationDefaultParameters _defaultParametersSo = null;

    public override void InstallBindings()
    {
        Container.Bind<PopupAnimationDefaultParameters>().FromInstance(_defaultParametersSo).AsSingle();
    }
}