using Model;
using Project.Aiming;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Enums;

namespace Project.Pieces
{
    public class BubbleFlyObserver : IBubbleFlyObserver
    {
        private readonly IGameStateController _gameStateController = null;
        private readonly PiecesData _piecesData = null;
        private readonly IAimingStartPointProvider _aimingStartPointProvider = null;
        
        public BubbleFlyObserver(IGameStateController gameStateController, PiecesData piecesData, IAimingStartPointProvider aimingStartPointProvider,
            IFindingCellToShootPieceController findingCellToShootPieceController)
        {
            _gameStateController = gameStateController;
            _piecesData = piecesData;
            _aimingStartPointProvider = aimingStartPointProvider;
            
            findingCellToShootPieceController.PieceFlyPath.Skip(1).Where(x => x.Length > 0).Subscribe(x =>
            {
                _gameStateController.ChangeGamePlayState(GamePlayState.BubbleFlying);
                ChangeStateAfterFlyDuration(x);
            });
        }

        private void ChangeStateAfterFlyDuration(Vector2[] flyPath)
        {
            var distance = CalculateDistanceOfFly(flyPath);
            var flySpeed = _piecesData.FlySpeed;
            var flyDuration = distance / flySpeed;

            DOVirtual.DelayedCall(flyDuration, () => { _gameStateController.ChangeGamePlayState(GamePlayState.PlacingBubbleOnGrid); });
        }

        private float CalculateDistanceOfFly(Vector2[] flyPath)
        {
            var distance = 0f;

            for (int i = 0; i < flyPath.Length; i++)
            {
                if (i == 0)
                {
                    var pathElement = flyPath[i] - _aimingStartPointProvider.GetAimingStartPoint();
                    distance += pathElement.magnitude;
                }
                else
                {
                    var pathElement = flyPath[i] - flyPath[i - 1];
                    distance += pathElement.magnitude;
                }
            }

            return distance;
        }
    }
}