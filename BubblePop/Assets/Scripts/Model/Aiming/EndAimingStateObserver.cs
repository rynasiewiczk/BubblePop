using Enums;
using Model;
using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public class EndAimingStateObserver : IEndAimingStateObserver
    {
        private readonly IBubbleDestinationFinder _bubbleDestinationFinder = null;
        private readonly IGameStateController _gameStateController = null;

        public EndAimingStateObserver(IInputEventsNotifier inputEventsNotifier, IBubbleDestinationFinder bubbleDestinationFinder,
            IGameStateController gameStateController)
        {
            _bubbleDestinationFinder = bubbleDestinationFinder;
            _gameStateController = gameStateController;

            inputEventsNotifier.OnInputEnd.Where(x => gameStateController.GamePlayState.Value == GamePlayState.Aiming)
                .Subscribe(x => FireBubbleIfFound());
        }

        private void FireBubbleIfFound()
        {
            if (_bubbleDestinationFinder.BubbleHitPoint != null)
            {
                Debug.Log("Spawn bubble on grid. Position of hit: " + _bubbleDestinationFinder.BubbleHitPoint);
                //it should change to bubble flying state
                _gameStateController.GamePlayState.Value = GamePlayState.Idle;
            }
            else
            {
                _gameStateController.GamePlayState.Value = GamePlayState.Idle;
            }
        }
    }
}