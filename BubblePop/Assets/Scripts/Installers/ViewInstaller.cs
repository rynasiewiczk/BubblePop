using UnityEngine;
using View;
using View.FlyingAfterAiming;
using Zenject;

public class ViewInstaller : MonoInstaller
{
    [SerializeField] private BubbleView _bubbleView = null;
    [SerializeField] private FlyingBubbleView _flyingBubbleView = null;

    public override void InstallBindings()
    {
        Container.BindMemoryPool<BubbleView, BubbleViewPool>()
            .WithInitialSize(70)
            .FromComponentInNewPrefab(_bubbleView)
            .UnderTransformGroup("BubbleViews");

        Container.DeclareSignal<OnBubbleHitSignal>();
        Container.BindInterfacesTo<ViewBounceAfterBubbleArriveController>().AsSingle().NonLazy();

        Container.BindMemoryPool<FlyingBubbleView, FlyingBubbleViewPool>()
            .WithInitialSize(1)
            .FromComponentInNewPrefab(_flyingBubbleView)
            .UnderTransformGroup("FlyingBubbleViews");

        Container.BindInterfacesTo<FlyingBubbleViewController>().AsSingle().NonLazy();
    }
}