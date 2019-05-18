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

        public NextBubbleLevelToSpawnController(IGameStateController gameStateController, IBubblesSpawner bubblesSpawner, BubbleData bubbleData)
        {
            BubbleLevelToSpawn = new ReactiveProperty<int>();

            _bubbleData = bubbleData;
            
            gameStateController.GamePlayState.Where(x => x == GamePlayState.None || x == GamePlayState.BubblesCombining).Subscribe(x => FindNextLevelToSpawn());
            FindNextLevelToSpawn();
        }

        private void FindNextLevelToSpawn()
        {
            var maxLevelToSpawn = _bubbleData.MaxBubbleLevelToSpawn;
            var levelToSpawn = Random.Range(1, maxLevelToSpawn + 1);

            BubbleLevelToSpawn.Value = levelToSpawn;
            Debug.Log("NEXT TO SPAWN: " + levelToSpawn);
        }
    }
}