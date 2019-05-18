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
            signalBus.Subscribe<SpawnBubbleOnGridSignal>(signal => GetScoreOnBubbleSpawn(signal.Level));

            _bubbleData = bubbleData;

            //todo: middle state for getting score would be useful
            gameStateController.GamePlayState.Where(x => x == GamePlayState.DropBubblesAfterCombining)
                .Subscribe(x => GetScoreOnBubbleSpawn(bubblesSpawner.JustSpawned.Value.Level.Value));
        }

        private void GetScoreOnBubbleSpawn(int level)
        {
            if (_gameStateController.GamePlayState.Value == GamePlayState.Idle || _gameStateController.GamePlayState.Value == GamePlayState.None)
            {
                return;
            }

            Score.Value += _bubbleData.GetValueForLevel(level);
        }
    }
}