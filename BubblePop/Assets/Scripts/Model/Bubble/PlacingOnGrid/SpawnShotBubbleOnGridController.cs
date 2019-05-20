using Enums;
using Model;
using Project.Aiming;
using UniRx;
using Zenject;

namespace Project.Bubbles.PlacingOnGrid
{
    public class SpawnShotBubbleOnGridController : ISpawnShotBubbleOnGridController
    {
        private readonly SpawnBubbleOnGridSignal _spawnBubbleOnGridSignal = new SpawnBubbleOnGridSignal();

        public SpawnShotBubbleOnGridController(IGameStateController gameStateController, IFindingCellToShootBubbleController findingCellToShootBubbleController,
            INextBubbleLevelToSpawnController nextBubbleLevelToSpawnController, SignalBus signalBus)
        {
            var gameStateController1 = gameStateController;

            gameStateController1.GamePlayState.Where(x => x == GamePlayState.PlacingBubbleOnGrid).Subscribe(x =>
            {
                _spawnBubbleOnGridSignal.Position = findingCellToShootBubbleController.BubbleDestination;
                _spawnBubbleOnGridSignal.Level = nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value;
                signalBus.Fire(_spawnBubbleOnGridSignal);
                
                gameStateController1.ChangeGamePlayState(GamePlayState.BubblesCombining);
            });
        }
    }
}