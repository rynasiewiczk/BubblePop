using Enums;
using Model;
using Project.Aiming;
using UniRx;
using Zenject;

namespace Project.Bubbles.PlacingOnGrid
{
    public class PlacerOfBubbleOnGrid : IPlacerOfBubbleOnGrid
    {
        private readonly SpawnBubbleOnGridSignal _spawnBubbleOnGridSignal = new SpawnBubbleOnGridSignal();

        public PlacerOfBubbleOnGrid(IGameStateController gameStateController, IEndAimingStateObserver endAimingStateObserver,
            INextBubbleLevelToSpawnController nextBubbleLevelToSpawnController, SignalBus signalBus)
        {
            var gameStateController1 = gameStateController;

            gameStateController1.GamePlayState.Where(x => x == GamePlayState.PlacingBubbleOnGrid).Subscribe(x =>
            {
                _spawnBubbleOnGridSignal.Position = endAimingStateObserver.BubbleDestination;
                _spawnBubbleOnGridSignal.Level = nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value;
                signalBus.Fire(_spawnBubbleOnGridSignal);
                gameStateController1.ChangeGamePlayState(GamePlayState.BubblesCombining);
            });
        }
    }
}