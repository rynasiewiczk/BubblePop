using Project.Pieces;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using View.FlyingAfterAiming;
using Zenject;

namespace View.Aiming.PrewarmBubbleDisplay
{
    public class AimingBubblesController : MonoBehaviour
    {
        [Inject] private readonly INextBubbleLevelToSpawnController _nextBubbleLevelToSpawnController = null;
        [Inject] private readonly SignalBus _signalBus = null;

        [FormerlySerializedAs("aimingBubble")] [SerializeField] private AimingBubble _aimingBubble = null;
        [FormerlySerializedAs("prewarmBubble")] [SerializeField] private SmallPrewarmBubble _prewarmBubble = null;

        private void Awake()
        {
            Debug.Assert(_aimingBubble, "Missing reference: _prewarmBubble", this);
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

            _signalBus.Subscribe<BubbleFlySignal>(signal => HideBubbleOnShot());
        }

        private void HideBubbleOnShot()
        {
            _aimingBubble.Hide();
        }

        private void ShowNewPrewarm(int bubbleLevel, bool instant)
        {
            _prewarmBubble.Show(bubbleLevel, instant);
        }

        private void SlideViewToAimPosition(int bubbleLevel, bool instant)
        {
            _aimingBubble.Show(bubbleLevel, instant);
        }
    }
}