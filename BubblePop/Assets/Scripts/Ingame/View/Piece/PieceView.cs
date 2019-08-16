using TMPro;
using UnityEngine;
using Zenject;

namespace View
{
    public class PieceView : MonoBehaviour
    {
        [Inject] private readonly PiecesData _piecesData = null;

        [SerializeField] private SpriteRenderer _innerCircle = null;
        [SerializeField] private SpriteRenderer _outerCircle = null;

        [SerializeField] private TextMeshPro _text = null;

        public void Setup(int level)
        {
            var set = _piecesData.GetColorForLevel(level);
            _innerCircle.color = set.InnerColor;
            _outerCircle.color = set.OuterColor;

            var stringValue = _piecesData.GetValueInDisplayFormatFromPieceLevel(level);
            _text.text = stringValue;
            _text.sortingOrder = 900;
        }

        public float GetAlpha()
        {
            var result = _innerCircle.color.a;
            return result;
        }

        public void SetAlpha(float alpha)
        {
            var innerColor = _innerCircle.color;
            innerColor.a = alpha;
            _innerCircle.color = innerColor;

            var outerColor = _outerCircle.color;
            outerColor.a = alpha;
            _outerCircle.color = outerColor;

            _text.alpha = alpha;
        }

        public Color GetMainColor()
        {
            var color = _innerCircle.color;
            color.a = 1;
            return color;
        }

        public void SetOrderLayerForRow(int row)
        {
            var outerCircleLayer = row * BubbleViewSettings.RENDER_LAYERS_MULTIPLAYER;
            _outerCircle.sortingOrder = outerCircleLayer;
            _innerCircle.sortingOrder = outerCircleLayer + 1;

            var textSortingOrder = row * BubbleViewSettings.RENDER_LAYERS_MULTIPLAYER + BubbleViewSettings.TEXT_RENDER_LAYER_ADDITION;
            _text.sortingOrder = textSortingOrder;
        }
    }
}