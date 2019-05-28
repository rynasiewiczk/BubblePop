using System;
using System.Collections.Generic;
using Enums;
using Model;
using Project.Grid;
using Project.Input;
using UniRx;
using UnityEngine;

namespace Project.Aiming
{
    //TODO: it has way to many responsibilities. break it down
    public class AimEndPointFinder : IAimEndPointFinder
    {
        private readonly string _topWallLayerName = SRLayers.TopWall.name;
        private readonly string _sideWallLayerName = SRLayers.SideWall.name;
        private readonly string _pieces = SRLayers.Piece.name;

        private readonly IAimingDirectionObserver _aimingDirectionObserver = null;
        private readonly IGridMap _gridMap = null;
        private readonly IAimingStartPointProvider _aimingStartPointProvider = null;
        private readonly AimingSettings _aimingSettings = null;

        private readonly LayerMask _layerMask;
        private const float ADDITION_TO_REFLECTION_POINT_TO_AVOID_CURRENT_WALL_HIT = .1f;
        public List<Vector2> AimPath { get; private set; } = new List<Vector2>();

        private Vector2 AimingPositionInWorldPoint => _aimingStartPointProvider.GetAimingStartPoint();

        public PieceAimedData AimedPieceData { get; private set; }

        public AimEndPointFinder(IGameStateController gameStateController, IInputEventsNotifier inputEventsNotifier,
            IAimingDirectionObserver aimingDirectionObserver, IGridMap gridMap, IAimingStartPointProvider aimingStartPointProvider,
            AimingSettings aimingSettings)
        {
            _layerMask = LayerMask.GetMask(_pieces, _sideWallLayerName, _topWallLayerName);
            _aimingDirectionObserver = aimingDirectionObserver;
            _gridMap = gridMap;
            _aimingStartPointProvider = aimingStartPointProvider;
            _aimingSettings = aimingSettings;

            inputEventsNotifier.OnInputStart.Where(x => /*gameStateController.GamePlayState.Value == GamePlayState.Aiming &&*/ AimingAboveStartingPoint())
                .Subscribe(x => FireRaycastToFindPositionForPiece(AimingPositionInWorldPoint, 0));

            inputEventsNotifier.OnInputMove.Where(x => gameStateController.GamePlayState.Value == GamePlayState.Aiming && AimingAboveStartingPoint())
                .Subscribe(x => FireRaycastToFindPositionForPiece(AimingPositionInWorldPoint, 0));
        }

        private bool AimingAboveStartingPoint()
        {
            return _aimingDirectionObserver.AimingDirection.Value.y > 0;
        }

        private void FireRaycastToFindPositionForPiece(Vector2 startPoint, int wallsReflections = 0)
        {
            if (wallsReflections == 0)
            {
                ResetAimData();
            }

            startPoint = AdjustStartPointToAvoidHitSameObject(startPoint);

            var aimingDirection = SetAimingDirection(wallsReflections);

            var raycastHit = GetHitCollider(startPoint, aimingDirection, out var collider);
            if (collider == null)
            {
                ResetAimData();
                return;
            }

            if (TopWallWasHit(collider))
            {
                var cellPosition =
                    _gridMap.GetCellPositionByWorldPosition(new Vector2(raycastHit.point.x, raycastHit.point.y + _gridMap.GetViewPosition(Vector2.up).y));
                var pieceAtPosition = _gridMap.GetPieceAtPositionOrNull(cellPosition);

                var hitPointRelativeToPiecesCenter = raycastHit.point - cellPosition;
                var aimedSide = hitPointRelativeToPiecesCenter.x > 0 ? PieceSide.BottomLeft : PieceSide.BottomRight;

                AimPath.Add(raycastHit.point);

                var invertedAimSide = aimedSide == PieceSide.BottomLeft ? PieceSide.BottomRight : PieceSide.BottomLeft;
                var finalPositionOnGrid = _gridMap.GetPositionToSpawnPiece(pieceAtPosition, invertedAimSide, cellPosition - raycastHit.point);
                var copyOfAimPathWithFinalPositionOnGrid = AimPath.ToArray();
                copyOfAimPathWithFinalPositionOnGrid[AimPath.Count - 1] = finalPositionOnGrid;
                AimedPieceData = new PieceAimedData(pieceAtPosition, aimedSide, copyOfAimPathWithFinalPositionOnGrid);

                return;
            }

            if (SideWallWasHit(collider))
            {
                if (wallsReflections >= _aimingSettings.MaxAmountOfWallBounces)
                {
                    ResetAimData();
                    return;
                }

                ResetAimData();

                var hitPosition = raycastHit.point;

                AimPath.Add(hitPosition);

                wallsReflections++;
                FireRaycastToFindPositionForPiece(hitPosition, wallsReflections);
            }

            else if (PieceWasHit(collider))
            {
                var piecePosition = collider.gameObject.transform.position;
                var aimedSide = GetPieceAimedSide(raycastHit.point, piecePosition);
                //todo: get position on row for the collider and get piece from GridMap
                var pieceView = collider.gameObject.GetComponentInParent<BubbleView>();

                AimPath.Add(raycastHit.point);

                var hitPointRelativeToPiecesCenter = raycastHit.point - (Vector2) collider.transform.position;
                var finalPositionOnGrid = _gridMap.GetPositionToSpawnPiece(pieceView.Model, aimedSide, hitPointRelativeToPiecesCenter);
                var copyOfAimPathWithFinalPositionOnGrid = AimPath.ToArray();
                copyOfAimPathWithFinalPositionOnGrid[AimPath.Count - 1] = finalPositionOnGrid;
                AimedPieceData = new PieceAimedData(pieceView.Model, aimedSide, copyOfAimPathWithFinalPositionOnGrid);
            }
            else
            {
                ResetAimData();
                Debug.LogWarning("Raycast hit not expected layer. Hit layer: " + collider.gameObject.layer);
            }
        }

        private bool TopWallWasHit(Collider2D collider)
        {
            return collider.gameObject.layer == LayerMask.NameToLayer(_topWallLayerName);
        }

        private bool SideWallWasHit(Collider2D collider)
        {
            return collider.gameObject.layer == LayerMask.NameToLayer(_sideWallLayerName);
        }

        private bool PieceWasHit(Collider2D collider)
        {
            return collider.gameObject.layer == LayerMask.NameToLayer(_pieces);
        }

        private void ResetAimData()
        {
            AimedPieceData = null;
            AimPath.Clear();
        }

        private PieceSide GetPieceAimedSide(Vector2 hitPoint, Vector2 piecePosition)
        {
            var result = PieceSide.None;

            var hitOffset = hitPoint - piecePosition;
            if (hitOffset.x < 0 && hitOffset.y < 0)
            {
                result = PieceSide.BottomLeft;
            }
            else if (hitOffset.x < 0 && hitOffset.y >= 0)
            {
                result = PieceSide.TopLeft;
            }
            else if (hitOffset.x >= 0 && hitOffset.y < 0)
            {
                result = PieceSide.BottomRight;
            }
            else if (hitOffset.x >= 0 && hitOffset.y >= 0)
            {
                result = PieceSide.TopRight;
            }

            if (result == PieceSide.None)
            {
                Debug.LogError("Side of piece hit was not calculated properly. Returning BottmLeft");
                return PieceSide.BottomLeft;
            }

            return result;
        }

        private RaycastHit2D GetHitCollider(Vector2 startPoint, Vector2 aimingDirection, out Collider2D collider)
        {
            var raycastHit = Physics2D.Raycast(startPoint, aimingDirection, Mathf.Infinity,
                _layerMask);

            collider = raycastHit.collider;
            return raycastHit;
        }

        private Vector2 SetAimingDirection(int reflections)
        {
            var aimingDirection = _aimingDirectionObserver.AimingDirection.Value;
            if (reflections % 2 == 1)
            {
                aimingDirection = new Vector2(-aimingDirection.x, aimingDirection.y);
            }

            return aimingDirection;
        }

        private Vector2 AdjustStartPointToAvoidHitSameObject(Vector2 startPoint)
        {
            if (Math.Abs(startPoint.x - _aimingDirectionObserver.AimingStartPosition.x) < ADDITION_TO_REFLECTION_POINT_TO_AVOID_CURRENT_WALL_HIT)
            {
                return startPoint;
            }

            if (startPoint.x - _aimingDirectionObserver.AimingStartPosition.x < 0)
            {
                startPoint = new Vector2(startPoint.x + ADDITION_TO_REFLECTION_POINT_TO_AVOID_CURRENT_WALL_HIT, startPoint.y);
            }
            else if (startPoint.x - _aimingDirectionObserver.AimingStartPosition.x > 0)
            {
                startPoint = new Vector2(startPoint.x - ADDITION_TO_REFLECTION_POINT_TO_AVOID_CURRENT_WALL_HIT, startPoint.y);
            }

            return startPoint;
        }
    }
}