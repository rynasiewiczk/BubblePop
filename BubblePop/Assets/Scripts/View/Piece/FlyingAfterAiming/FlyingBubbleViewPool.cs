using Project.Aiming;
using UnityEngine;
using Zenject;

namespace View.FlyingAfterAiming
{
    public class FlyingBubbleViewPool : MonoMemoryPool<FlyingBubbleView>
    {
        private readonly IAimingStartPointProvider _aimingStartPointProvider = null;

        public FlyingBubbleViewPool(IAimingStartPointProvider aimingStartPointProvider)
        {
            _aimingStartPointProvider = aimingStartPointProvider;
        }

        protected override void Reinitialize(FlyingBubbleView item)
        {
            item.transform.position = _aimingStartPointProvider.GetAimingStartPoint();
        }
    }
}