using Model;
using UnityEngine;

namespace Project.Aiming
{
    public class AimingStartPointProvider : IAimingStartPointProvider
    {
        private readonly AimingSettings _aimingSettings = null;
        private readonly IGameplayCamera _gameplayCamera = null;

        public AimingStartPointProvider(IGameplayCamera gameplayCamera, AimingSettings aimingSettings)
        {
            _gameplayCamera = gameplayCamera;
            _aimingSettings = aimingSettings;
        }

        public Vector2 GetAimingStartPoint()
        {
            var viewPortAimingPosition = _aimingSettings.GetAimingPositionInViewPortPosition();
            var result = _gameplayCamera.Camera.ViewportToWorldPoint(viewPortAimingPosition);
            return result;
        }
    }
}