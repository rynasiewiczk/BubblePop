using UnityEngine;

namespace View
{
    public class BubbleViewLayering : MonoBehaviour
    {
        [SerializeField] private GridBubble _view = null;

        [SerializeField] private PieceView _pieceView = null;

        private int _latestRecordedRow = -1;

        private void Awake()
        {
            Debug.Assert(_view, "Missing reference: _view", this);
            Debug.Assert(_pieceView, "Missing reference: _pieceView", this);
        }

        private void Update()
        {
            var model = _view.Model;
            if (model == null)
            {
                return;
            }

            if (_latestRecordedRow != model.Position.Value.y)
            {
                _latestRecordedRow = model.Position.Value.y;
                UpdateLayerOfRenderers(model.Position.Value.y);
            }
        }

        private void OnDisable()
        {
            _latestRecordedRow = -1;
        }

        private void UpdateLayerOfRenderers(int row)
        {
            _pieceView.SetOrderLayerForRow(row);
        }
    }
}