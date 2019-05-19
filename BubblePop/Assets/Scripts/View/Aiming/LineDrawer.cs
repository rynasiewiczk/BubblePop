using System.Collections.Generic;
using Enums;
using Model;
using Project.Aiming;
using UnityEngine;
using Zenject;

namespace View.Aiming
{
    public class LineDrawer : MonoBehaviour
    {
        [Inject] private readonly AimingSettings _aimingSettings = null;
        [Inject] private readonly Camera _camera = null;
        [Inject] private readonly IBubbleDestinationFinder _bubbleDestinationFinder = null;
        [Inject] private readonly IGameStateController _gameStateController = null;

        [SerializeField] private LineRenderer _line = null;

        private void Awake()
        {
            Debug.Assert(_line, "Missing reference: _line", this);
        }

        private void Update()
        {
            if (_gameStateController.GamePlayState.Value != GamePlayState.Aiming)
            {
                return;
            }

            DrawLine(_bubbleDestinationFinder.AimPath);
        }

        private void DrawLine(List<Vector2> path)
        {
            if (path.Count == 0 || path.Count > 2)
            {
                return;
            }

            _line.positionCount = path.Count + 1;
            var startPoint = _camera.ViewportToWorldPoint(_aimingSettings.GetAimingPositionInViewPortPosition());
            startPoint = new Vector3(startPoint.x, startPoint.y, 0);
            _line.SetPosition(0, startPoint);

            for (int i = 0; i < path.Count; i++)
            {
                _line.SetPosition(i + 1, path[i]);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}