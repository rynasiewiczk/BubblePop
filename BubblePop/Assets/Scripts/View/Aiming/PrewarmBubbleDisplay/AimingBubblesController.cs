using Project.Pieces;
using UniRx;
using UnityEngine;
using Zenject;

namespace View.Aiming.PrewarmBubbleDisplay
{
    public class AimingBubblesController : MonoBehaviour
    {
        [Inject] private readonly INextBubbleLevelToSpawnController _nextBubbleLevelToSpawnController = null;
        [Inject] private readonly PiecesData _piecesData = null;

        [SerializeField] private AimingBubble aimingBubble = null;
        [SerializeField] private SmallPrewarmBubble prewarmBubble = null;

        private void Awake()
        {
            Debug.Assert(aimingBubble, "Missing reference: _prewarmBubble", this);
        }

        private void Start()
        {
            _nextBubbleLevelToSpawnController.BubblesToSpawnUpdated.Subscribe(x =>
            {
                ShowNewPrewarm(_nextBubbleLevelToSpawnController.PreWarmedBubbleLevelToSpawn.Value, false);
                SlideViewToAimPosition(_nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value, false);
            });

            ShowNewPrewarm(_nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value, true);
            SlideViewToAimPosition(_nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value, true);
        }

        private void ShowNewPrewarm(int bubbleLevel, bool instant)
        {
            var color = _piecesData.GetColorForLevel(bubbleLevel);
            var value = _piecesData.GetValueForLevel(bubbleLevel);
            prewarmBubble.Show(color, value, instant);
        }


        private void SlideViewToAimPosition(int bubbleLevel, bool instant)
        {
            var color = _piecesData.GetColorForLevel(bubbleLevel);
            var value = _piecesData.GetValueForLevel(bubbleLevel);
            aimingBubble.Show(color, value, instant);
        }
    }
}