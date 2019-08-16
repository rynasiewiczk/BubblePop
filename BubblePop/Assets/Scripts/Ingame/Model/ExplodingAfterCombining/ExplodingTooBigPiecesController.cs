using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Model;
using Model.Progress.PlayerLevelController;
using Project.Grid;
using Project.Pieces;
using UniRx;
using UnityEngine;
using View.DestroyParticles;
using Zenject;

namespace Ingame.Model.ExplodingAfterCombining
{
    public class ExplodingTooBigPiecesController : IExplodingTooBigPiecesController
    {
        private readonly IGridMap _gridMap = null;
        private readonly PiecesData _piecesData = null;
        private readonly IPlayerLevelController _playerLevelController = null;

        [Inject] private readonly PieceDestroyOnOvergrownExplosionParticlesPool _pieceDestroyOnOvergrownExplosionParticlesPool = null;


        private List<IPiece> _bufferList = new List<IPiece>();

        public ExplodingTooBigPiecesController(IGameStateController gameStateController, IGridMap gridMap, PiecesData piecesData,
            IPlayerLevelController playerLevelController)
        {
            _gridMap = gridMap;
            _piecesData = piecesData;
            _playerLevelController = playerLevelController;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.DropAndExplodePiecesAfterCombining).Subscribe(x => ExplodeTooBigPieces());
        }

        private void ExplodeTooBigPieces()
        {
            var playerLevel = _playerLevelController.PlayerLevel.Value;
            var maxAllowedPieceLevelOnGrid = _piecesData.GetBiggestPieceLevelToLiveOnGrid(playerLevel);

            _bufferList.Clear();

            var tooBigToken = _gridMap.GetFirstTokenEqualOrBiggerThanValueOrNull(maxAllowedPieceLevelOnGrid);
            if (tooBigToken == null)
            {
                return;
            }

            tooBigToken.Destroy();

            var color = _piecesData.GetColorsSetForLevel(tooBigToken.Level.Value).InnerColor;

            var destroyParticles = _pieceDestroyOnOvergrownExplosionParticlesPool.Spawn();
            var viewPosition = _gridMap.GetViewPosition(tooBigToken.Position.Value);
            destroyParticles.Setup(color, viewPosition, DestroyParticlesSourceType.ExplodeOvergrown);
        }
    }
}