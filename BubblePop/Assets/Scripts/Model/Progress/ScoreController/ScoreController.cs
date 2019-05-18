using System;
using Enums;
using Project.Bubbles;
using UniRx;
using Zenject;

namespace Model.ScoreController
{
    public class ScoreController : IScoreController, IDisposable
    {
        public ReactiveProperty<int> Score { get; }

        private readonly BubbleData _bubbleData = null;
        private IGameStateController _gameStateController = null;
        private SignalBus _signalBus = null;

        public ScoreController(IBubblesSpawner bubblesSpawner, IGameStateController gameStateController, SignalBus signalBus, BubbleData bubbleData)
        {
            Score = new ReactiveProperty<int>();

            _gameStateController = gameStateController;
            _signalBus = signalBus;
            _signalBus.Subscribe<SpawnBubbleOnGridSignal>(signal => GetScoreOnBubbleCombine(signal.Level));

            _bubbleData = bubbleData;
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SpawnBubbleOnGridSignal>(signal => GetScoreOnBubbleCombine(signal.Level));
        }

        private void GetScoreOnBubbleCombine(int level)
        {
            if (_gameStateController.GamePlayState.Value < GamePlayState.BubblesCombining)
            {
                return;
            }

            Score.Value += _bubbleData.GetValueForLevel(level);
        }
    }
}