using System;
using Enums;
using Model;
using Project.Bubbles;
using UniRx;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

namespace DefaultNamespace
{
    public class BubbleDestinationFinder : IBubbleDestinationFinder
    {
        private readonly string _wallLayerName = SRLayers.Wall.name;
        private readonly string _bubbleLayerName = SRLayers.Bubble.name;

        public ReactiveProperty<Vector2Int> DestinationPosition { get; }

        private readonly IAimingDirectionObserver _aimingDirectionObserver = null;
        private readonly LayerMask _layerMask;

        //public BubbleSide BubbleHitSide { get; private set; }
        public BubbleAimedData BubbleAimedData { get; private set; }

        public BubbleDestinationFinder(IGameStateController gameStateController, IInputEventsNotifier inputEventsNotifier,
            IAimingDirectionObserver aimingDirectionObserver)
        {
            _layerMask = LayerMask.GetMask(_bubbleLayerName, _wallLayerName);
            _aimingDirectionObserver = aimingDirectionObserver;

            inputEventsNotifier.OnInputMove.Where(x => gameStateController.GamePlayState.Value == GamePlayState.Aiming)
                .Subscribe(x => FireRaycastToFindPositionForBubble(x, 0));

            inputEventsNotifier.OnInputEnd.Skip(1).Subscribe(x => Debug.Log(BubbleAimedData.AimedSide.ToString()));
        }

        private void FireRaycastToFindPositionForBubble(Vector2 startPoint, int reflections = 0)
        {
            if (AimingBelowTheLimit())
            {
                return;
            }


            startPoint = AdjustStartPointToAvoidHitSameObject(startPoint);

            var aimingDirection = SetAimingDirection(reflections);

            var raycastHit = GetHitCollider(startPoint, aimingDirection, out var collider);
            if (collider == null)
            {
                return;
            }

            Debug.DrawLine(startPoint, raycastHit.point, Color.magenta);

            if (collider.gameObject.layer == LayerMask.NameToLayer(_wallLayerName))
            {
                BubbleAimedData = null;

                var hitPosition = raycastHit.point;
                reflections++;
                FireRaycastToFindPositionForBubble(hitPosition, reflections);
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer(_bubbleLayerName))
            {
                var bubblePosition = collider.gameObject.transform.position;
                var aimedSide = GetBubbleAimedSide(raycastHit.point, bubblePosition);
                //todo: get position on row for the collider and get bubble from GridMap
                var bubbleView = collider.gameObject.GetComponentInParent<BubbleView>();
                BubbleAimedData = new BubbleAimedData(bubbleView.Model, aimedSide);
            }
            else
            {
                BubbleAimedData = null;
                Debug.LogWarning("Raycast hit not expected layer. Hit layer: " + collider.gameObject.layer);
            }
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

        private bool AimingBelowTheLimit()
        {
            var result = _aimingDirectionObserver.AimingDirection.Value.y < 0;
            return result;
        }
    }
}