using Enums;
using Project.Bubbles;
using UniRx;
using Zenject;

namespace Model.ScoreController
{
    public class ScoreController : IScoreController
    {
        public ReactiveProperty<int> Score { get; }

        private readonly BubbleData _bubbleData = null;
        private IGameStateController _gameStateController = null;

        public ScoreController(IBubblesSpawner bubblesSpawner, IGameStateController gameStateController, SignalBus signalBus, BubbleData bubbleData)
        {
            Score = new ReactiveProperty<int>();

            _gameStateController = gameStateController;
            signalBus.Subscribe<SpawnBubbleOnGridSignal>(signal => GetScoreOnBubbleCombine(signal.Level));

            _bubbleData = bubbleData;

            //todo: middle state for getting score would be useful
//            gameStateController.GamePlayState.Where(x => x == GamePlayState.DropBubblesAfterCombining)
//                .Subscribe(x => GetScoreOnBubbleCombine(bubblesSpawner.JustSpawned.Value.Level.Value));
            //bubblesSpawner.JustSpawned./*Where(x => _gameStateController.GamePlayState.Value >= GamePlayState.BubblesCombining && x != null)
            //    .*/Subscribe(x => GetScoreOnBubbleSpawn(x.Level.Value));
        }

        private void GetScoreOnBubbleCombine(int level)
        {
            if (_gameStateController.GamePlayState.Value >= GamePlayState.Aiming && _gameStateController.GamePlayState.Value < GamePlayState.DropBubblesAfterCombining)
            {
                return;
            }

            Score.Value += _bubbleData.GetValueForLevel(level);
        }
    }
}