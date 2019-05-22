using Enums;
using Model.Progress.PlayerLevelController;
using Project.Pieces;
using Project.Grid;
using UniRx;
using Zenject;

namespace Model.FillingBubblesAbovePlayspace
{
    public class FillingBubblesAbovePlayspaceController : IFillingBubblesAbovePlayspaceController
    {
        private readonly IGridMap _gridMap = null;
        private readonly PiecesData _piecesData = null;
        private readonly IGameStateController _gameStateController = null;
        
        private readonly SignalBus _signalBus = null;
        private readonly SpawnPieceOnGridSignal _spawnPieceOnGridSignal = new SpawnPieceOnGridSignal();

        private readonly int _gridHeight;
        private readonly ReadOnlyReactiveProperty<int> _playerLevel;

        public FillingBubblesAbovePlayspaceController(IGridMap gridMap, GridSettings gridSettings, PiecesData piecesData, SignalBus signalBus,
            IGameStateController gameStateController, IPlayerLevelController playerLevelController)
        {
            _gridMap = gridMap;
            _gridHeight = gridSettings.StartGridSize.y;
            
            _piecesData = piecesData;
            _signalBus = signalBus;
            _gameStateController = gameStateController;
            _playerLevel = new ReadOnlyReactiveProperty<int>(playerLevelController.PlayerLevel);

            gameStateController.GamePlayState.Where(x => x == GamePlayState.FillBubblesAboveTop).Subscribe(x => FillBubbles());
        }

        private void FillBubbles()
        {
            var allEmptyCellsAboveTheGrid = _gridMap.GetAllEmptyCellsAboveTheGrid(_gridHeight);

            foreach (var cell in allEmptyCellsAboveTheGrid)
            {
                _spawnPieceOnGridSignal.Position = cell.Position;
                _spawnPieceOnGridSignal.Level = _piecesData.GetRandomPieceLevelBasedOnPlayerLevel(_playerLevel.Value);
                _signalBus.Fire(_spawnPieceOnGridSignal);
            }

            _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
        }
    }
}