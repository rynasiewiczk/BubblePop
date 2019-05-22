using System;
using Enums;
using Model.CombiningBubbles.DroppingDisconnectedBubbles;
using Project.Pieces;
using UniRx;
using Zenject;

namespace Model.ScoreController
{
    public class ScoreController : IScoreController, IDisposable
    {
        public ReactiveProperty<int> Score { get; }

        private readonly PiecesData _piecesData = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly SignalBus _signalBus = null;

        public ScoreController(IGameStateController gameStateController, SignalBus signalBus, PiecesData piecesData)
        {
            Score = new ReactiveProperty<int>();

            _gameStateController = gameStateController;
            _signalBus = signalBus;

            _signalBus.Subscribe<SpawnPieceOnGridSignal>(signal => GetScoreOnBubbleCombine(signal.Level));
            _signalBus.Subscribe<DroppingUnlinkedBubbleSignal>(signal => GetScoreOnBubbleDrop(signal.Level));

            _piecesData = piecesData;
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SpawnPieceOnGridSignal>(signal => GetScoreOnBubbleCombine(signal.Level));
            _signalBus.TryUnsubscribe<DroppingUnlinkedBubbleSignal>(signal => GetScoreOnBubbleDrop(signal.Level));
        }

        private void GetScoreOnBubbleCombine(int level)
        {
            if (_gameStateController.GamePlayState.Value < GamePlayState.BubblesCombining ||
                _gameStateController.GamePlayState.Value >= GamePlayState.FillBubblesAboveTop)
            {
                return;
            }

            Score.Value += _piecesData.GetValueForLevel(level);
        }

        private void GetScoreOnBubbleDrop(int level)
        {
            Score.Value += _piecesData.GetValueForLevel(level);
        }
    }
}