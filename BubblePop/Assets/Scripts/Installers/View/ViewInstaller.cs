using UnityEngine;
using View;
using View.DestroyParticles;
using View.DroppingUnconnected;
using View.FlyingAfterAiming;
using Zenject;

public class ViewInstaller : MonoInstaller
{
    [SerializeField] private BubbleView _bubbleView = null;
    [SerializeField] private FlyingBubbleView _flyingBubbleView = null;
    [SerializeField] private DroppingBubbleView _droppingBubbleView = null;
    [SerializeField] private PieceDestroyParticles _pieceDestroyOnCombineParticles = null;
    [SerializeField] private PieceDestroyParticles _pieceDestroyOnDropParticles = null;

    public override void InstallBindings()
    {
        Container.BindMemoryPool<BubbleView, BubbleViewPool>()
            .WithInitialSize(50)
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

        Container.BindInterfacesTo<PieceDestroyOnCombineParticlesController>().AsSingle().NonLazy();
        Container.BindMemoryPool<PieceDestroyParticles, PieceDestroyOnCombineParticlesPool>()
            .WithInitialSize(6)
            .FromComponentInNewPrefab(_pieceDestroyOnCombineParticles)
            .UnderTransformGroup("PieceDestroyOnCollapseParticles");
        Container.BindMemoryPool<PieceDestroyParticles, PieceDestroyOnDropParticlesPool>()
            .WithInitialSize(6)
            .FromComponentInNewPrefab(_pieceDestroyOnDropParticles)
            .UnderTransformGroup("PieceDestroyParticles");
    }
}