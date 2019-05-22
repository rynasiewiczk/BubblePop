using Enums;
using Model.Progress.PlayerLevelController;
using Project.Bubbles;
using Project.Grid;
using UniRx;
using Zenject;

namespace Model.FillingBubblesAbovePlayspace
{
    public class FillingBubblesAbovePlayspaceController : IFillingBubblesAbovePlayspaceController
    {
        private readonly IGridMap _gridMap = null;
        private readonly BubbleData _bubbleData = null;
        private readonly IGameStateController _gameStateController = null;
        
        private readonly SignalBus _signalBus = null;
        private readonly SpawnBubbleOnGridSignal _spawnBubbleOnGridSignal = new SpawnBubbleOnGridSignal();

        private readonly int _gridHeight;
        private readonly ReadOnlyReactiveProperty<int> _playerLevel;

        public FillingBubblesAbovePlayspaceController(IGridMap gridMap, GridSettings gridSettings, BubbleData bubbleData, SignalBus signalBus,
            IGameStateController gameStateController, IPlayerLevelController playerLevelController)
        {
            _gridMap = gridMap;
            _gridHeight = gridSettings.StartGridSize.y;
            
            _bubbleData = bubbleData;
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
                _spawnBubbleOnGridSignal.Position = cell.Position;
                _spawnBubbleOnGridSignal.Level = _bubbleData.GetRandomBubbleLevelBasedOnPlayerLevel(_playerLevel.Value);
                _signalBus.Fire(_spawnBubbleOnGridSignal);
            }

            _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
        }
    }
}