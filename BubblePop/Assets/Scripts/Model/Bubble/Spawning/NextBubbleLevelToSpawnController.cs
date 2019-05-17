using Enums;
using Model;
using UniRx;
using UnityEngine;

namespace Project.Bubbles
{
    public class NextBubbleLevelToSpawnController : INextBubbleLevelToSpawnController
    {
        public ReactiveProperty<int> BubbleLevelToSpawn { get; private set; }

        private readonly BubbleData _bubbleData = null;

        public NextBubbleLevelToSpawnController(IGameStateController gameStateController, BubbleData bubbleData)
        {
            _bubbleData = bubbleData;
            gameStateController.GamePlayState.Where(x => x == GamePlayState.None || x == GamePlayState.BubbleFlying).Subscribe(x => FindNextLevelToSpawn());
        }

        private void FindNextLevelToSpawn()
        {
            var maxLevelToSpawn = _bubbleData.MaxBubbleLevelToSpawn;
            var levelToSpawn = Random.Range(1, maxLevelToSpawn + 1);

            BubbleLevelToSpawn.Value = levelToSpawn;
        }
    }
}