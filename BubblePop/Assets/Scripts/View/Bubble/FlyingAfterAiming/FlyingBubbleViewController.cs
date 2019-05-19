using System.Collections.Generic;
using DG.Tweening;
using Project.Aiming;
using Project.Bubbles;
using Project.Grid;
using UniRx;
using UnityEngine;

namespace View.FlyingAfterAiming
{
    public class FlyingBubbleViewController : IFlyingBubbleViewController
    {
        private const float DELAY_FOR_SMOOTHNESS = 0.07f;
        private readonly IEndAimingStateObserver _endAimingStateObserver = null;
        private readonly INextBubbleLevelToSpawnController _nextBubbleLevelToSpawnController = null;
        private readonly BubbleData _bubbleData = null;
        private readonly IGridMap _gridMap = null;
        private readonly AimingSettings _aimingSettings = null;
        private readonly Camera _camera;

        private readonly FlyingBubbleViewPool _flyingBubbleViewPool = null;
        private readonly List<Vector2> _pathCopyList = new List<Vector2>();

        public FlyingBubbleViewController(IEndAimingStateObserver endAimingStateObserver, INextBubbleLevelToSpawnController nextBubbleLevelToSpawnController,
            FlyingBubbleViewPool flyingBubbleViewPool, BubbleData bubbleData, IGridMap gridMap, AimingSettings aimingSettings, Camera camera)
        {
            _endAimingStateObserver = endAimingStateObserver;
            _nextBubbleLevelToSpawnController = nextBubbleLevelToSpawnController;
            _flyingBubbleViewPool = flyingBubbleViewPool;
            _bubbleData = bubbleData;
            _gridMap = gridMap;
            _aimingSettings = aimingSettings;
            _camera = camera;

            _endAimingStateObserver.BubbleFlyPath.Where(x => x != null && x.Length > 0).Subscribe(SpawnView);
        }

        private void SpawnView(Vector2[] path)
        {
            var level = _nextBubbleLevelToSpawnController.BubbleLevelToSpawn.Value;
            var flyingBubbleView = _flyingBubbleViewPool.Spawn();

            var path3d = new Vector3[path.Length];

            _pathCopyList.Clear();
            for (int i = 0; i < path.Length; i++)
            {
                _pathCopyList.Add(new Vector2(path[i].x, path[i].y));
            }

            for (int i = 0; i < _pathCopyList.Count; i++)
            {
                if (i == _pathCopyList.Count - 1)
                {
                    _pathCopyList[i] = _gridMap.GetGridViewPosition(_endAimingStateObserver.BubbleDestination);
                }

                path3d[i] = _pathCopyList[i];
            }

            var duration = GetFlyDuration(path, path3d);
            var bubbleValue = _bubbleData.GetValueForLevel(level);
            var color = _bubbleData.GetColorForLevel(level);

            flyingBubbleView.Setup(path3d, bubbleValue, color, duration,
                () => { DOVirtual.DelayedCall(DELAY_FOR_SMOOTHNESS, () => { _flyingBubbleViewPool.Despawn(flyingBubbleView); }); });
        }

        private float GetFlyDuration(Vector2[] path, Vector3[] path3d)
        {
            var flySpeed = _bubbleData.FlySpeed;
            var distance = GetDistanceToCover(path, path3d);
            var duration = distance / flySpeed;
            return duration;
        }

        private float GetDistanceToCover(Vector2[] path, Vector3[] path3d)
        {
            var startPosition = _camera.ViewportToWorldPoint(_aimingSettings.GetAimingPositionInViewPortPosition());
            var distance = 0f;
            distance = ((Vector2) path3d[0] - (Vector2) startPosition).magnitude;
            for (int i = 1; i < path.Length; i++)
            {
                distance += (path[i] - path[i - 1]).magnitude;
            }

            return distance;
        }
    }
}