using Enums;
using Project.Bubbles;
using UniRx;
using UnityEngine;

namespace Model.ScoreController
{
    public class ScoreController : IScoreController
    {
        public ReactiveProperty<int> Score { get; }

        private readonly BubbleData _bubbleData = null;

        public ScoreController(IBubblesSpawner bubblesSpawner, IGameStateController gameStateController, BubbleData bubbleData)
        {
            Score = new ReactiveProperty<int>();

            _bubbleData = bubbleData;

            //todo: middle state for getting score would be useful
            gameStateController.GamePlayState.Where(x => x == GamePlayState.DropBubblesAfterCombining)
                .Subscribe(x => GetScoreOnDestroy(bubblesSpawner.JustSpawned.Value));
            //bubblesSpawner.JustSpawned.Where(x => x != null).Subscribe(GetScoreOnDestroy);
        }

        private void GetScoreOnDestroy(IBubble bubble)
        {
            bubble.Destroyed.Subscribe(x =>
            {
                Score.Value += _bubbleData.GetValueForLevel(x.Level.Value);
                Debug.Log(Score.Value);
            });
        }
    }
}