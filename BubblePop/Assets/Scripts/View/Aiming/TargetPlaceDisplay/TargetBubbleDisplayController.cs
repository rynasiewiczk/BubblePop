using Enums;
using Model;
using Project.Bubbles;
using UniRx;
using UnityEngine;
using Zenject;

namespace View.Aiming.TargetPlaceDisplay
{
    public class TargetBubbleDisplayController : MonoBehaviour
    {
        [Inject] private readonly IGameStateController _gameStateController = null;
        [Inject] private readonly INextBubbleLevelToSpawnController _nextBubbleLevelToSpawnController = null;
        [Inject] private readonly BubbleData _bubbleData = null;

        [SerializeField] private TargetBubbleDisplay _display = null;

        private void Awake()
        {
            Debug.Assert(_display, "Missing reference: _display", this);
        }

        private void Start()
        {
            _gameStateController.GamePlayState.Where(x => x == GamePlayState.Aiming).Subscribe(x => ShowTargetDisplay());
            _gameStateController.GamePlayState.Where(x => x != GamePlayState.Aiming).Subscribe(x => HideTargetDisplay());

            _nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Subscribe(UpdateTargetDisplayColor);
        }

        private void UpdateTargetDisplayColor(int level)
        {
            var color = _bubbleData.GetColorForLevel(level);
            _display.SetColor(color);
        }

        private void HideTargetDisplay()
        {
            _display.Hide();
        }

        private void ShowTargetDisplay()
        {
            _display.Show();
        }
        

    }
}