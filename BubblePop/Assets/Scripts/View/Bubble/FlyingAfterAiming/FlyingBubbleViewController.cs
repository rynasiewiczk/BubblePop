using System.Collections.Generic;
using DG.Tweening;
using Project.Aiming;
using Project.Pieces;
using Project.Grid;
using UniRx;
using UnityEngine;

namespace View.FlyingAfterAiming
{
    public class FlyingBubbleViewController : IFlyingBubbleViewController
    {
        private const float DELAY_TO_KILL_AFTER_GRID_VIEW_IS_VISIBLE = 0.1f;
        private readonly IFindingCellToShootBubbleController _findingCellToShootBubbleController = null;
        private readonly INextBubbleLevelToSpawnController _nextBubbleLevelToSpawnController = null;
        private readonly PiecesData _piecesData = null;
        private readonly IGridMap _gridMap = null;
        private readonly IAimingStartPointProvider _aimingStartPointProvider = null;
        
        private readonly FlyingBubbleViewPool _flyingBubbleViewPool = null;
        private readonly List<Vector2> _pathCopyList = new List<Vector2>();

        public FlyingBubbleViewController(IFindingCellToShootBubbleController findingCellToShootBubbleController, INextBubbleLevelToSpawnController nextBubbleLevelToSpawnController,
            FlyingBubbleViewPool flyingBubbleViewPool, PiecesData piecesData, IGridMap gridMap, IAimingStartPointProvider aimingStartPointProvider)
        {
            _findingCellToShootBubbleController = findingCellToShootBubbleController;
            _nextBubbleLevelToSpawnController = nextBubbleLevelToSpawnController;
            _flyingBubbleViewPool = flyingBubbleViewPool;
            _piecesData = piecesData;
            _gridMap = gridMap;
            _aimingStartPointProvider = aimingStartPointProvider;

            _findingCellToShootBubbleController.BubbleFlyPath.Where(x => x != null && x.Length > 0).Subscribe(SpawnView);
        }

        private void SpawnView(Vector2[] path)
        {
            var level = _nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value;
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
                    _pathCopyList[i] = _gridMap.GetCellsViewPosition(_findingCellToShootBubbleController.BubbleDestination);
                }

                path3d[i] = _pathCopyList[i];
            }

            var duration = GetFlyDuration(path, path3d);
            var bubbleValueValueToDisplay = _piecesData.GetValueInDisplayFormatFromPieceLevel(level, 0);
            var color = _piecesData.GetColorForLevel(level);

            flyingBubbleView.Setup(path3d, bubbleValueValueToDisplay, color, duration,
                () => { DOVirtual.DelayedCall(DELAY_TO_KILL_AFTER_GRID_VIEW_IS_VISIBLE, () => { _flyingBubbleViewPool.Despawn(flyingBubbleView); }); });
        }

        private float GetFlyDuration(Vector2[] path, Vector3[] path3d)
        {
            var flySpeed = _piecesData.FlySpeed;
            var distance = GetDistanceToCover(path, path3d);
            var duration = distance / flySpeed;
            return duration;
        }

        private float GetDistanceToCover(Vector2[] path, Vector3[] path3d)
        {
            var startPosition = _aimingStartPointProvider.GetAimingStartPoint();
            var distance = 0f;
            distance = ((Vector2) path3d[0] - startPosition).magnitude;
            for (int i = 1; i < path.Length; i++)
            {
                distance += (path[i] - path[i - 1]).magnitude;
            }

            return distance;
        }
    }
}