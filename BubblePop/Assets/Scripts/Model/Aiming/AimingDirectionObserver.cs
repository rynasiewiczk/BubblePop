using Enums;
using Model;
using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public class AimingDirectionObserver : IAimingDirectionObserver
    {
        public ReactiveProperty<Vector2> AimingDirection { get; }
        public Vector2 AimingStartPosition => _camera.ViewportToWorldPoint(_aimingSettings.GetAimingPositionInViewPortPosition());

        private readonly AimingSettings _aimingSettings = null;
        private readonly Camera _camera = null;
        private bool _trackAimDirection;


        public AimingDirectionObserver(IGameStateController gameStateController, IInputEventsNotifier inputEventsNotifier, AimingSettings aimingSettings,
            Camera camera)
        {
            AimingDirection = new ReactiveProperty<Vector2>();

            _aimingSettings = aimingSettings;
            _camera = camera;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.Aiming).Subscribe(x => _trackAimDirection = true);
            gameStateController.GamePlayState.Where(x => x != GamePlayState.Aiming).Subscribe(x => _trackAimDirection = false);

            inputEventsNotifier.OnInputMove.Subscribe(GetAimDirection);
        }

        private void GetAimDirection(Vector2 inputPosition)
        {
            if (!_trackAimDirection)
            {
                return;
            }

            var direction = (inputPosition - AimingStartPosition);
            AimingDirection.Value = direction.normalized;
        }
    }
}