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
        public int LastGainedScore { get; private set; }

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
            if (_gameStateController.GamePlayState.Value < GamePlayState.PiecesCombining ||
                _gameStateController.GamePlayState.Value >= GamePlayState.FillPiecesAboveTop)
            {
                return;
            }

            UpdateScores(level);
        }

        private void GetScoreOnBubbleDrop(int level)
        {
            UpdateScores(level);
        }

        private void UpdateScores(int level)
        {
            var valueForLevel = _piecesData.GetValueForLevel(level);
            UpdateLastGainedScore(Score.Value - valueForLevel);
            Score.Value += valueForLevel;
        }

        private void UpdateLastGainedScore(int score)
        {
            LastGainedScore = score;
        }
    }
}