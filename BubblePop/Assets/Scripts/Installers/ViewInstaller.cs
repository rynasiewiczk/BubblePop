using UnityEngine;
using View;
using View.DroppingUnconnected;
using View.FlyingAfterAiming;
using Zenject;

public class ViewInstaller : MonoInstaller
{
    [SerializeField] private BubbleView _bubbleView = null;
    [SerializeField] private FlyingBubbleView _flyingBubbleView = null;
    [SerializeField] private DroppingBubbleView _droppingBubbleView = null;

    public override void InstallBindings()
    {
        Container.BindMemoryPool<BubbleView, BubbleViewPool>()
            .WithInitialSize(60)
            .FromComponentInNewPrefab(_bubbleView)
            .UnderTransformGroup("BubbleViews");

        Container.DeclareSignal<OnBubbleHitSignal>();
        Container.BindInterfacesTo<ViewBounceAfterBubbleArriveController>().AsSingle().NonLazy();

        Container.BindMemoryPool<FlyingBubbleView, FlyingBubbleViewPool>()
            .WithInitialSize(1)
            .FromComponentInNewPrefab(_flyingBubbleView)
            .UnderTransformGroup("FlyingBubbleViews");

        Container.BindInterfacesTo<FlyingBubbleViewController>().AsSingle().NonLazy();

        Container.BindMemoryPool<DroppingBubbleView, DroppingBubbleViewPool>()
            .WithInitialSize(5)
            .FromComponentInNewPrefab(_droppingBubbleView)
            .UnderTransformGroup("DroppingBubbleViews");

        Container.BindInterfacesTo<DroppingUnconnectedBubblesViewController>().AsSingle().NonLazy();
    }
}