using System.Collections.Generic;
using Calculations;
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

        private readonly List<Vector2> _flyPath = new List<Vector2>(3);

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

        private void ChangeStateAfterFlyDuration(Vector2[] pathWithoutStartPoint)
        {
            _flyPath.Clear();
            _flyPath.Add(_aimingStartPointProvider.GetAimingStartPoint());
            _flyPath.AddRange(pathWithoutStartPoint);
            
            var distance = VectorsHelper.SumMagnitudeOfVectors(_flyPath);
            var flySpeed = _piecesData.FlySpeed;
            var flyDuration = distance / flySpeed;

            DOVirtual.DelayedCall(flyDuration, () => { _gameStateController.ChangeGamePlayState(GamePlayState.PlacingBubbleOnGrid); }, false);
        }
    }
}