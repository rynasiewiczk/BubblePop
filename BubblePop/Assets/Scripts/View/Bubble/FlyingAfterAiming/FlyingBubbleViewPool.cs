using UnityEngine;
using Zenject;

namespace View.FlyingAfterAiming
{
    public class FlyingBubbleViewPool : MonoMemoryPool< /*Vector2[], int,*/ FlyingBubbleView>
    {
        private AimingSettings _aimingSettings = null;
        private Camera _camera = null;

        public FlyingBubbleViewPool(AimingSettings aimingSettings, Camera camera)
        {
            _aimingSettings = aimingSettings;
            _camera = camera;
        }

        protected override void Reinitialize(FlyingBubbleView item)
        {
            var viewportPosition = _aimingSettings.GetAimingPositionInViewPortPosition();
            var worldPosition = _camera.ViewportToWorldPoint(viewportPosition);

            item.transform.position = worldPosition;
        }
    }
}