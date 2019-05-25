using Enums;
using Model;
using Project.Aiming;
using UniRx;
using Zenject;

namespace Project.Pieces.PlacingOnGrid
{
    public class SpawnShotBubbleOnGridController : ISpawnShotBubbleOnGridController
    {
        private readonly SpawnPieceOnGridSignal _spawnPieceOnGridSignal = new SpawnPieceOnGridSignal();

        public SpawnShotBubbleOnGridController(IGameStateController gameStateController, IFindingCellToShootPieceController findingCellToShootPieceController,
            INextBubbleLevelToSpawnController nextBubbleLevelToSpawnController, SignalBus signalBus)
        {
            var gameStateController1 = gameStateController;

            gameStateController1.GamePlayState.Where(x => x == GamePlayState.PlacingBubbleOnGrid).Subscribe(x =>
            {
                _spawnPieceOnGridSignal.Position = findingCellToShootPieceController.PieceDestinationPosition;
                _spawnPieceOnGridSignal.Level = nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value;
                signalBus.Fire(_spawnPieceOnGridSignal);
                
                gameStateController1.ChangeGamePlayState(GamePlayState.BubblesCombining);
            });
        }
    }
}