using Enums;
using Model;
using Model.Progress.PlayerLevelController;
using UniRx;
using UnityEngine;

namespace Project.Pieces
{
    public class NextBubbleLevelToSpawnController : INextBubbleLevelToSpawnController
    {
        private const int FIRST_PREWARMED_BUBBLE_LEVEL = 2;
        private const int FIRST_LEVEL_TO_SPAWN = 1;
        
        public ReactiveProperty<int> NextBubbleLevelToSpawn { get; private set; }
        public ReactiveProperty<int> PreWarmedBubbleLevelToSpawn { get; private set; }
        public ReactiveCommand BubblesToSpawnUpdated { get; private set; }


        private readonly IPlayerLevelController _playerLevelController = null;
        private readonly PiecesData _piecesData = null;

        public NextBubbleLevelToSpawnController(IGameStateController gameStateController, PiecesData piecesData, IPlayerLevelController playerLevelController,
            IBubblesSpawner bubblesSpawner)
        {
            BubblesToSpawnUpdated = new ReactiveCommand();
            NextBubbleLevelToSpawn = new ReactiveProperty<int>(FIRST_LEVEL_TO_SPAWN);
            PreWarmedBubbleLevelToSpawn = new ReactiveProperty<int>(FIRST_PREWARMED_BUBBLE_LEVEL);

            _piecesData = piecesData;
            _playerLevelController = playerLevelController;

            bubblesSpawner.LatestSpawnedBubble.Where(x => gameStateController.GamePlayState.Value < GamePlayState.PiecesCombining)
                .Subscribe(x => FindNextLevelToSpawn());
            FindNextLevelToSpawn();
        }

        private void FindNextLevelToSpawn()
        {
            var maxLevelToSpawn = _piecesData.GetRandomPieceLevelBasedOnPlayerLevel(_playerLevelController.PlayerLevel.Value);
            var levelToSpawn = Random.Range(1, maxLevelToSpawn + 1);

            NextBubbleLevelToSpawn.Value = PreWarmedBubbleLevelToSpawn.Value;
            PreWarmedBubbleLevelToSpawn.Value = levelToSpawn;

            BubblesToSpawnUpdated.Execute();
        }
    }
}