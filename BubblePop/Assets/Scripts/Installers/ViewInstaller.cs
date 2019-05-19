using UnityEngine;
using View;
using Zenject;

public class ViewInstaller : MonoInstaller
{
    [SerializeField] private BubbleView _bubbleView = null;

    public override void InstallBindings()
    {
        Container.BindMemoryPool<BubbleView, BubbleViewPool>()
            .WithInitialSize(70)
            .FromComponentInNewPrefab(_bubbleView)
            .UnderTransformGroup("BubbleViews");

        Container.DeclareSignal<OnBubbleHitSignal>();
        Container.BindInterfacesTo<ViewBounceAfterBubbleArriveController>().AsSingle().NonLazy();
    }
}