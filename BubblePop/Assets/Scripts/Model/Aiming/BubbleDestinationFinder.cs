using System;
using Enums;
using Model;
using UniRx;
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

        public Vector2? BubbleHitPoint { get; private set; }

        public BubbleDestinationFinder(IGameStateController gameStateController, IInputEventsNotifier inputEventsNotifier,
            IAimingDirectionObserver aimingDirectionObserver)
        {
            _layerMask = LayerMask.GetMask(_bubbleLayerName, _wallLayerName);
            _aimingDirectionObserver = aimingDirectionObserver;

            inputEventsNotifier.OnInputMove.Where(x => gameStateController.GamePlayState.Value == GamePlayState.Aiming)
                .Subscribe(x => FireRaycastToFindPositionForBubble(x, 0));
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
                BubbleHitPoint = null;

                var hitPosition = raycastHit.point;
                reflections++;
                FireRaycastToFindPositionForBubble(hitPosition, reflections);
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer(_bubbleLayerName))
            {
                BubbleHitPoint = raycastHit.point;

                Debug.DrawLine((Vector2) BubbleHitPoint, (Vector2) BubbleHitPoint + Vector2.down / 2, Color.red);
                Debug.DrawLine((Vector2) BubbleHitPoint, (Vector2) BubbleHitPoint + Vector2.left / 2, Color.red);
                Debug.DrawLine((Vector2) BubbleHitPoint, (Vector2) BubbleHitPoint + Vector2.right / 2, Color.red);
                Debug.DrawLine((Vector2) BubbleHitPoint, (Vector2) BubbleHitPoint + Vector2.up / 2, Color.red);
            }
            else
            {
                BubbleHitPoint = null;
                Debug.LogWarning("Raycast hit not expected layer. Hit layer: " + collider.gameObject.layer);
            }
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