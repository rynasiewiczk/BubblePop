using Model;
using Project.Aiming;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    public class BubbleFlyObserver : IBubbleFlyObserver
    {
        private readonly IGameStateController _gameStateController = null;
        private readonly BubbleData _bubbleData = null;
        
        public BubbleFlyObserver(IGameStateController gameStateController, BubbleData bubbleData, IEndAimingStateObserver endAimingStateObserver)
        {
            _gameStateController = gameStateController;
            _bubbleData = bubbleData;
            
            endAimingStateObserver.BubbleFlyPath.Skip(1).Where(x => x.Length > 0).Subscribe(ChangeStateAfterFlyDuration);
        }

        private void ChangeStateAfterFlyDuration(Vector2[] flyPath)
        {
            var distance = 0f;
            flyPath.ForEach(x => distance += x.magnitude);

            var flySpeed = _bubbleData.FlySpeed;

            var flyDuration = distance / flySpeed;
        }
    }
}