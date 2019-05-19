using Enums;
using Model.Progress.PlayerLevelController;
using Project.Bubbles;
using Project.Grid;
using UniRx;
using UnityEngine;
using Zenject;

namespace Model.FillingBubblesAbovePlayspace
{
    public class FillingBubblesAbovePlayspaceController : IFillingBubblesAbovePlayspaceController
    {
        private IGridMap _gridMap = null;
        private GridSettings _gridSettings = null;
        private BubbleData _bubbleData = null;
        private PlayerLevelSettings _playerLevelSettings;
        private SignalBus _signalBus = null;
        private IGameStateController _gameStateController = null;
        private SpawnBubbleOnGridSignal _spawnBubbleOnGridSignal = new SpawnBubbleOnGridSignal();
        private IPlayerLevelController _playerLevelController = null;


        public FillingBubblesAbovePlayspaceController(IGridMap gridMap, GridSettings gridSettings, BubbleData bubbleData, SignalBus signalBus,
            IGameStateController gameStateController, IPlayerLevelController playerLevelController)
        {
            _gridMap = gridMap;
            _gridSettings = gridSettings;
            _bubbleData = bubbleData;
            _signalBus = signalBus;
            _gameStateController = gameStateController;
            _playerLevelController = playerLevelController;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.FillBubblesAboveTop).Subscribe(x => FillBubbles());
        }

        private void FillBubbles()
        {
            var allEmptyCellsAboveTheGrid = _gridMap.GetAllEmptyCellsAboveTheGrid(_gridSettings.StartGridSize.y);

            foreach (var cell in allEmptyCellsAboveTheGrid)
            {
                _spawnBubbleOnGridSignal.Position = cell.Position;
                _spawnBubbleOnGridSignal.Level = _bubbleData.GetRandomBubbleLevelBasedOnPlayerLevel(_playerLevelController.PlayerLevel.Value);
                Debug.Log(_spawnBubbleOnGridSignal.Level);
                _signalBus.Fire(_spawnBubbleOnGridSignal);
            }

            _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
        }
    }
}