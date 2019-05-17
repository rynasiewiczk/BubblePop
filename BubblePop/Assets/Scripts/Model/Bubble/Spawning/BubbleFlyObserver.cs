using Model;
using Project.Aiming;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Enums;

namespace Project.Bubbles
{
    public class BubbleFlyObserver : IBubbleFlyObserver
    {
        private readonly IGameStateController _gameStateController = null;
        private readonly BubbleData _bubbleData = null;
        private readonly AimingSettings _aimingSettings = null;
        private readonly Camera _camera = null;

        public BubbleFlyObserver(IGameStateController gameStateController, BubbleData bubbleData, AimingSettings aimingSettings, Camera camera,
            IEndAimingStateObserver endAimingStateObserver)
        {
            _gameStateController = gameStateController;
            _bubbleData = bubbleData;
            _aimingSettings = aimingSettings;
            _camera = camera;

            endAimingStateObserver.BubbleFlyPath.Skip(1).Where(x => x.Length > 0).Subscribe(x =>
            {
                _gameStateController.ChangeGamePlayState(GamePlayState.BubbleFlying);
                ChangeStateAfterFlyDuration(x);
            });
        }

        private void ChangeStateAfterFlyDuration(Vector2[] flyPath)
        {
            var distance = CalculateDistanceOfFly(flyPath);
            var flySpeed = _bubbleData.FlySpeed;
            var flyDuration = distance / flySpeed;

            DOVirtual.DelayedCall(flyDuration, () => { _gameStateController.ChangeGamePlayState(GamePlayState.Idle); });
        }

        private float CalculateDistanceOfFly(Vector2[] flyPath)
        {
            var distance = 0f;

            for (int i = 0; i < flyPath.Length; i++)
            {
                if (i == 0)
                {
                    var pathElement = flyPath[i] - (Vector2) _camera.ViewportToWorldPoint(_aimingSettings.GetAimingPositionInViewPortPosition());
                    distance += pathElement.magnitude;
                }
                else
                {
                    var pathElement = flyPath[i] - flyPath[i - 1];
                    distance += pathElement.magnitude;
                }
            }

            return distance;
        }
    }
}