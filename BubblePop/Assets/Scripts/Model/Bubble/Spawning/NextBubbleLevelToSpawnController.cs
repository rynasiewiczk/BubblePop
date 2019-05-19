using Enums;
using Model;
using Model.Progress.PlayerLevelController;
using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    public class NextBubbleLevelToSpawnController : INextBubbleLevelToSpawnController
    {
        public ReactiveProperty<int> BubbleLevelToSpawn { get; private set; }

        private IPlayerLevelController _playerLevelController = null;
        private readonly BubbleData _bubbleData = null;

        public NextBubbleLevelToSpawnController(IGameStateController gameStateController, BubbleData bubbleData, IPlayerLevelController playerLevelController)
        {
            BubbleLevelToSpawn = new ReactiveProperty<int>();

            _bubbleData = bubbleData;
            _playerLevelController = playerLevelController;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.None || x == GamePlayState.BubblesCombining).Subscribe(x => FindNextLevelToSpawn());
            FindNextLevelToSpawn();
        }

        private void FindNextLevelToSpawn()
        {
            var maxLevelToSpawn = _bubbleData.GetRandomBubbleLevelBasedOnPlayerLevel(_playerLevelController.PlayerLevel.Value);
            var levelToSpawn = Random.Range(1, maxLevelToSpawn + 1);

            BubbleLevelToSpawn.Value = levelToSpawn;
        }
    }
}