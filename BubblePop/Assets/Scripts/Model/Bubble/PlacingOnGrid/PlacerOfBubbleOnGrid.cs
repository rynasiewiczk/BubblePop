using Enums;
using Model;
using Project.Aiming;
using UniRx;
using Zenject;

namespace Project.Bubbles.PlacingOnGrid
{
    public class PlacerOfBubbleOnGrid : IPlacerOfBubbleOnGrid
    {
        private readonly IGameStateController _gameStateController = null;
        private readonly SpawnBubbleOnGridSignal _spawnBubbleOnGridSignal = new SpawnBubbleOnGridSignal();

        public PlacerOfBubbleOnGrid(IGameStateController gameStateController, IEndAimingStateObserver endAimingStateObserver,
            INextBubbleLevelToSpawnController nextBubbleLevelToSpawnController, SignalBus signalBus)
        {
            _gameStateController = gameStateController;

            _gameStateController.GamePlayState.Where(x => x == GamePlayState.PlacingBubbleOnGrid).Subscribe(x =>
            {
                _spawnBubbleOnGridSignal.Position = endAimingStateObserver.BubbleDestination;
                _spawnBubbleOnGridSignal.Level = nextBubbleLevelToSpawnController.BubbleLevelToSpawn.Value;
                signalBus.Fire(_spawnBubbleOnGridSignal);
                _gameStateController.ChangeGamePlayState(GamePlayState.BubblesCombining);
            });
        }
    }
}