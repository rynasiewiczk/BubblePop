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
        [Inject] private readonly IAimEndPointFinder _aimEndPointFinder = null;
        [Inject] private readonly IGameStateController _gameStateController = null;
        [Inject] private readonly IAimingStartPointProvider _aimingStartPointProvider = null;

        [SerializeField] private LineRenderer _line = null;

        private void Awake()
        {
            Debug.Assert(_line, "Missing reference: _line", this);
        }

        private void Update()
        {
            if (_gameStateController.GamePlayState.Value != GamePlayState.Aiming)
            {
                DisableLine();
                return;
            }

            DrawLine(_aimEndPointFinder.AimPath);
        }

        private void DrawLine(List<Vector2> path)
        {
            if (path.Count == 0 || path.Count > 2)
            {
                DisableLine();
                return;
            }

            EnableLine();
            _line.positionCount = path.Count + 1;
            var startPoint = _aimingStartPointProvider.GetAimingStartPoint();
            startPoint = new Vector3(startPoint.x, startPoint.y, 0);
            _line.SetPosition(0, startPoint);

            for (int i = 0; i < path.Count; i++)
            {
                _line.SetPosition(i + 1, path[i]);
            }
        }

        private void EnableLine()
        {
            _line.enabled = true;
        }

        private void DisableLine()
        {
            _line.enabled = false;
        }

        public void Show()
        {
            EnableLine();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            DisableLine();
            gameObject.SetActive(false);
        }
    }
}