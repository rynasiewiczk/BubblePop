using System.Collections.Generic;
using System.Linq;
using Project.Aiming;
using UnityEngine;
using Zenject;

namespace View.Aiming
{
    public class LineDrawer : MonoBehaviour
    {
        [Inject] private AimingSettings _aimingSettings = null;
        [Inject] private Camera _camera = null;
        [Inject] private IBubbleDestinationFinder _bubbleDestinationFinder = null;

        [SerializeField] private LineRenderer _line;

        private void Awake()
        {
            Debug.Assert(_line, "Missing reference: _line", this);
        }

        private void Update()
        {
            DrawLine(_bubbleDestinationFinder.AimPath);
        }

        private void DrawLine(List<Vector2> path)
        {
            _line.positionCount = path.Count + 1;
            if (path == null || path.Count == 0 && path.Count > 2)
            {
                return;
            }

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