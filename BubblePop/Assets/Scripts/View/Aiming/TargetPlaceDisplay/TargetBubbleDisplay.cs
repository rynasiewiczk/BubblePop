using Project.Aiming;
using Project.Grid;
using UnityEngine;
using Zenject;

namespace View.Aiming.TargetPlaceDisplay
{
    public class TargetBubbleDisplay : MonoBehaviour
    {
        [Inject] private readonly IBubbleDestinationFinder _bubbleDestinationFinder = null;
        [Inject] private IGridMap _gridMap = null;

        [SerializeField] private SpriteRenderer _spriteRenderer = null;

        private void Awake()
        {
            Debug.Assert(_spriteRenderer, "Missing reference: _spriteRenderer", this);
        }

        private void Update()
        {
            var aimedBubbleData = _bubbleDestinationFinder.AimedBubbleData;
            if (aimedBubbleData == null)
            {
                return;
            }

            var destination = aimedBubbleData.Path[aimedBubbleData.Path.Length - 1];
            destination = _gridMap.GetGridViewPosition(new Vector2Int((int) destination.x, (int) destination.y));
            transform.position = destination;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetColor(Color color)
        {
            color.a = _spriteRenderer.color.a;
            _spriteRenderer.color = color;
        }
    }
}