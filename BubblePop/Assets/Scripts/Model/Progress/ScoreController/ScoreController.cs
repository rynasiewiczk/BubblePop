using System;
using Enums;
using Model.CombiningBubbles.DroppingDisconnectedBubbles;
using Project.Bubbles;
using UniRx;
using UnityEngine;
using Zenject;

namespace Model.ScoreController
{
    public class ScoreController : IScoreController, IDisposable
    {
        public ReactiveProperty<int> Score { get; }

        private readonly BubbleData _bubbleData = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly SignalBus _signalBus = null;

        public ScoreController(IGameStateController gameStateController, SignalBus signalBus, BubbleData bubbleData)
        {
            Score = new ReactiveProperty<int>();

            _gameStateController = gameStateController;
            _signalBus = signalBus;

            _signalBus.Subscribe<SpawnBubbleOnGridSignal>(signal => GetScoreOnBubbleCombine(signal.Level));
            _signalBus.Subscribe<DroppingUnlinkedBubbleSignal>(signal => GetScoreOnBubbleDrop(signal.Level));

            _bubbleData = bubbleData;
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SpawnBubbleOnGridSignal>(signal => GetScoreOnBubbleCombine(signal.Level));
            _signalBus.TryUnsubscribe<DroppingUnlinkedBubbleSignal>(signal => GetScoreOnBubbleDrop(signal.Level));
        }

        private void GetScoreOnBubbleCombine(int level)
        {
            if (_gameStateController.GamePlayState.Value < GamePlayState.BubblesCombining ||
                _gameStateController.GamePlayState.Value >= GamePlayState.FillBubblesAboveTop)
            {
                return;
            }

            Score.Value += _bubbleData.GetValueForLevel(level);
        }

        private void GetScoreOnBubbleDrop(int level)
        {
            Score.Value += _bubbleData.GetValueForLevel(level);
        }
    }
}