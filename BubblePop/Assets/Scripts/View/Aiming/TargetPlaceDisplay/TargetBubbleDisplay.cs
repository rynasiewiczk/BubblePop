using Project.Aiming;
using Project.Grid;
using UnityEngine;
using Zenject;

namespace View.Aiming.TargetPlaceDisplay
{
    public class TargetBubbleDisplay : MonoBehaviour
    {
        [Inject] private readonly IBubbleDestinationFinder _bubbleDestinationFinder = null;
        [Inject] private readonly IGridMap _gridMap = null;

        [SerializeField] private SpriteRenderer _spriteRenderer = null;

        private void Awake()
        {
            Debug.Assert(_spriteRenderer, "Missing reference: _spriteRenderer", this);
        }

        private void Update()
        {
            if (Time.time < 1f)
            {
                DisableRenderer();
                return;
            }

            var aimedBubbleData = _bubbleDestinationFinder.AimedBubbleData;
            if (aimedBubbleData == null)
            {
                DisableRenderer();
                return;
            }

            EnableRenderer();


            var destination = aimedBubbleData.Path[aimedBubbleData.Path.Length - 1];
            destination = _gridMap.GetGridViewPosition(new Vector2Int((int) destination.x, (int) destination.y));
            transform.position = destination;
        }

        private void EnableRenderer()
        {
            _spriteRenderer.enabled = true;
        }

        private void DisableRenderer()
        {
            _spriteRenderer.enabled = false;
        }

        public void Show()
        {
            EnableRenderer();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            DisableRenderer();
            gameObject.SetActive(false);
        }

        public void SetColor(Color color)
        {
            color.a = _spriteRenderer.color.a;
            _spriteRenderer.color = color;
        }
    }
}