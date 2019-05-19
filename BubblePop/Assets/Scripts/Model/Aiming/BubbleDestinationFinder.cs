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
    public class BubbleDestinationFinder : IBubbleDestinationFinder
    {
        private readonly string _wallLayerName = SRLayers.Wall.name;
        private readonly string _bubbleLayerName = SRLayers.Bubble.name;

        private readonly IAimingDirectionObserver _aimingDirectionObserver = null;
        private readonly IGridMap _gridMap = null;
        private readonly AimingSettings _aimingSettings = null;
        private readonly Camera _camera = null;

        private readonly LayerMask _layerMask;
        public List<Vector2> AimPath { get; private set; } = new List<Vector2>();

        private Vector2 AimingPositionInWorldPoint => _camera.ViewportToWorldPoint(_aimingSettings.GetAimingPositionInViewPortPosition());

        public BubbleAimedData AimedBubbleData { get; private set; }

        public BubbleDestinationFinder(IGameStateController gameStateController, IInputEventsNotifier inputEventsNotifier,
            IAimingDirectionObserver aimingDirectionObserver, AimingSettings aimingSettings, IGridMap gridMap, Camera camera)
        {
            _layerMask = LayerMask.GetMask(_bubbleLayerName, _wallLayerName);
            _aimingDirectionObserver = aimingDirectionObserver;
            _aimingSettings = aimingSettings;
            _gridMap = gridMap;
            _camera = camera;

            inputEventsNotifier.OnInputMove.Where(x => gameStateController.GamePlayState.Value == GamePlayState.Aiming && AimingAboveStartingPoint())
                .Subscribe(x => FireRaycastToFindPositionForBubble(AimingPositionInWorldPoint, 0));

            inputEventsNotifier.OnInputEnd.Subscribe(x => ResetAimData());
        }

        private bool AimingAboveStartingPoint()
        {
            return _aimingDirectionObserver.AimingDirection.Value.y > 0;
        }

        private void FireRaycastToFindPositionForBubble(Vector2 startPoint, int reflections = 0)
        {
            if (reflections == 0)
            {
                ResetAimData();
            }

            startPoint = AdjustStartPointToAvoidHitSameObject(startPoint);

            var aimingDirection = SetAimingDirection(reflections);

            var raycastHit = GetHitCollider(startPoint, aimingDirection, out var collider);
            if (collider == null)
            {
                ResetAimData();

                return;
            }

            if (collider.gameObject.layer == LayerMask.NameToLayer(_wallLayerName))
            {
                if (reflections >= _aimingSettings.MaxAmountOfWallBounces)
                {
                    ResetAimData();
                    return;
                }

                ResetAimData();

                var hitPosition = raycastHit.point;

                AimPath.Add(hitPosition);

                reflections++;
                FireRaycastToFindPositionForBubble(hitPosition, reflections);
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer(_bubbleLayerName))
            {
                var bubblePosition = collider.gameObject.transform.position;
                var aimedSide = GetBubbleAimedSide(raycastHit.point, bubblePosition);
                //todo: get position on row for the collider and get bubble from GridMap
                var bubbleView = collider.gameObject.GetComponentInParent<BubbleView>();

//                AimPath.Add(_gridMap.GetPositionToSpawnBubble(bubbleView.Model, aimedSide));
                AimPath.Add(raycastHit.point);
                AimedBubbleData = new BubbleAimedData(bubbleView.Model, aimedSide, AimPath.ToArray());
            }
            else
            {
                ResetAimData();
                Debug.LogWarning("Raycast hit not expected layer. Hit layer: " + collider.gameObject.layer);
            }
        }

        private void ResetAimData()
        {
            AimedBubbleData = null;
            AimPath.Clear();
        }

        private BubbleSide GetBubbleAimedSide(Vector2 hitPoint, Vector2 bubblePosition)
        {
            var result = BubbleSide.None;

            var hitOffset = hitPoint - bubblePosition;
            if (hitOffset.x < 0 && hitOffset.y < 0)
            {
                result = BubbleSide.BottomLeft;
            }
            else if (hitOffset.x < 0 && hitOffset.y >= 0)
            {
                result = BubbleSide.TopLeft;
            }
            else if (hitOffset.x >= 0 && hitOffset.y < 0)
            {
                result = BubbleSide.BottomRight;
            }
            else if (hitOffset.x >= 0 && hitOffset.y >= 0)
            {
                result = BubbleSide.TopRight;
            }

            if (result == BubbleSide.None)
            {
                Debug.LogError("Side of bubble hit was not calculated properly. Returning BottmLeft");
                return BubbleSide.BottomLeft;
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
            if (Math.Abs(startPoint.x - _aimingDirectionObserver.AimingStartPosition.x) < .1f) { }
            else if (startPoint.x - _aimingDirectionObserver.AimingStartPosition.x < 0)
            {
                startPoint = new Vector2(startPoint.x + .1f, startPoint.y);
            }
            else if (startPoint.x - _aimingDirectionObserver.AimingStartPosition.x > 0)
            {
                startPoint = new Vector2(startPoint.x + -.1f, startPoint.y);
            }

            return startPoint;
        }
    }
}