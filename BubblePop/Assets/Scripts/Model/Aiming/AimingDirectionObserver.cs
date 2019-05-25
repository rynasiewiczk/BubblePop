using Enums;
using Model;
using Project.Input;
using UniRx;
using UnityEngine;

namespace Project.Aiming
{
    public class AimingDirectionObserver : IAimingDirectionObserver
    {
        public ReactiveProperty<Vector2> AimingDirection { get; }

        private readonly IAimingStartPointProvider _aimingStartPointProvider;
        public Vector2 AimingStartPosition => _aimingStartPointProvider.GetAimingStartPoint();
        private bool _trackAimDirection;

        public AimingDirectionObserver(IGameStateController gameStateController, IInputEventsNotifier inputEventsNotifier,
            IAimingStartPointProvider aimingStartPointProvider)
        {
            AimingDirection = new ReactiveProperty<Vector2>();

            _aimingStartPointProvider = aimingStartPointProvider;

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