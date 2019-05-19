using Enums;
using Model;
using Project.Aiming;
using UniRx;
using View;
using Zenject;

namespace Project.Bubbles.PlacingOnGrid
{
    public class PlacerOfBubbleOnGrid : IPlacerOfBubbleOnGrid
    {
        private readonly IGameStateController _gameStateController = null;
        private readonly SpawnBubbleOnGridSignal _spawnBubbleOnGridSignal = new SpawnBubbleOnGridSignal();
        private readonly SignalBus _signalBus = null;
        private OnBubbleHitSignal _onBubbleHitSignal = new OnBubbleHitSignal();

        public PlacerOfBubbleOnGrid(IGameStateController gameStateController, IEndAimingStateObserver endAimingStateObserver,
            INextBubbleLevelToSpawnController nextBubbleLevelToSpawnController, SignalBus signalBus)
        {
            _gameStateController = gameStateController;

            _gameStateController.GamePlayState.Where(x => x == GamePlayState.PlacingBubbleOnGrid).Subscribe(x =>
            {
                _spawnBubbleOnGridSignal.Position = endAimingStateObserver.BubbleDestination;
                _spawnBubbleOnGridSignal.Level = nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value;
                signalBus.Fire(_spawnBubbleOnGridSignal);
                _gameStateController.ChangeGamePlayState(GamePlayState.BubblesCombining);
            });
        }
    }
}