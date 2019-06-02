using System.Collections.Generic;
using Calculations;
using Project.Aiming;
using Project.Pieces;
using Project.Grid;
using UniRx;
using UnityEngine;

namespace View.FlyingAfterAiming
{
    public class FlyingBubbleViewController : IFlyingBubbleViewController
    {
        private readonly IFindingCellToShootPieceController _findingCellToShootPieceController = null;
        private readonly INextBubbleLevelToSpawnController _nextBubbleLevelToSpawnController = null;
        private readonly PiecesData _piecesData = null;
        private readonly IGridMap _gridMap = null;
        private readonly IAimingStartPointProvider _aimingStartPointProvider = null;

        private readonly FlyingBubbleViewPool _flyingBubbleViewPool = null;
        private readonly List<Vector2> _pathCopyList = new List<Vector2>();
        private readonly List<Vector2> _fullFlyPath = new List<Vector2>();

        public FlyingBubbleViewController(IFindingCellToShootPieceController findingCellToShootPieceController,
            INextBubbleLevelToSpawnController nextBubbleLevelToSpawnController,
            FlyingBubbleViewPool flyingBubbleViewPool, PiecesData piecesData, IGridMap gridMap, IAimingStartPointProvider aimingStartPointProvider)
        {
            _findingCellToShootPieceController = findingCellToShootPieceController;
            _nextBubbleLevelToSpawnController = nextBubbleLevelToSpawnController;
            _flyingBubbleViewPool = flyingBubbleViewPool;
            _piecesData = piecesData;
            _gridMap = gridMap;
            _aimingStartPointProvider = aimingStartPointProvider;

            _findingCellToShootPieceController.PieceFlyPath.Where(x => x != null && x.Length > 0).Subscribe(SpawnView);
        }

        private void SpawnView(Vector2[] path)
        {
            var level = _nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value;
            var flyingBubbleView = _flyingBubbleViewPool.Spawn();


            _pathCopyList.Clear();
            for (int i = 0; i < path.Length; i++)
            {
                _pathCopyList.Add(new Vector2(path[i].x, path[i].y));
            }

            var path3d = new Vector3[path.Length];
            for (int i = 0; i < _pathCopyList.Count; i++)
            {
                if (i == _pathCopyList.Count - 1)
                {
                    _pathCopyList[i] = _gridMap.GetViewPosition(_findingCellToShootPieceController.PieceDestinationPosition);
                }

                path3d[i] = _pathCopyList[i];
            }

            var duration = GetFlyDuration(_aimingStartPointProvider.GetAimingStartPoint(), path);
            var bubbleValueValueToDisplay = _piecesData.GetValueInDisplayFormatFromPieceLevel(level, 0);
            var color = _piecesData.GetColorForLevel(level);

            flyingBubbleView.Setup(path3d, bubbleValueValueToDisplay, color, duration);
        }

        private float GetFlyDuration(Vector2 startPoint, Vector2[] path)
        {
            _fullFlyPath.Clear();
            _fullFlyPath.Add(startPoint);
            _fullFlyPath.AddRange(path);

            var flySpeed = _piecesData.FlySpeed;
            var distance = VectorsHelper.SumMagnitudeOfVectors(_fullFlyPath);
            var duration = distance / flySpeed;
            duration += Time.deltaTime; //extra frame to spawn piece before respawning fly view

            return duration;
        }
    }
}