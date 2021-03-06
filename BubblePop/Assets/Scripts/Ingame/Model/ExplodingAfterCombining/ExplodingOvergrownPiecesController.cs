using System.Collections.Generic;
using Enums;
using Model;
using Model.Progress.PlayerLevelController;
using Project.Grid;
using Project.Pieces;
using UniRx;
using View.DestroyParticles;
using Zenject;

namespace Ingame.Model.ExplodingAfterCombining
{
    public class ExplodingOvergrownPiecesController : IExplodingTooBigPiecesController
    {
        private readonly IGridMap _gridMap = null;
        private readonly PiecesData _piecesData = null;
        private readonly IPlayerLevelController _playerLevelController = null;

        private readonly PieceDestroyOnCombineParticlesPool _pieceDestroyOnCombineParticlesPool = null;
        private readonly PieceDestroyOnOvergrownExplosionParticlesPool _pieceDestroyOnOvergrownExplosionParticlesPool = null;
        
        private readonly SignalBus _signalBus = null;
        private readonly OvergrownExplosionSignal _overgrownExplosionSignal = new OvergrownExplosionSignal();
        
        private readonly List<IPiece> _bufferList = new List<IPiece>(6);

        public ExplodingOvergrownPiecesController(IGameStateController gameStateController, IGridMap gridMap, PiecesData piecesData,
            IPlayerLevelController playerLevelController, PieceDestroyOnCombineParticlesPool pieceDestroyOnCombineParticlesPool,
            PieceDestroyOnOvergrownExplosionParticlesPool pieceDestroyOnOvergrownExplosionParticlesPool, SignalBus signalBus)
        {
            _gridMap = gridMap;
            _piecesData = piecesData;
            _playerLevelController = playerLevelController;
            _pieceDestroyOnCombineParticlesPool = pieceDestroyOnCombineParticlesPool;
            _pieceDestroyOnOvergrownExplosionParticlesPool = pieceDestroyOnOvergrownExplosionParticlesPool;
            _signalBus = signalBus;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.DropAndExplodePiecesAfterCombining).Subscribe(x => ExplodeTooBigPieces());
        }

        private void ExplodeTooBigPieces()
        {
            var playerLevel = _playerLevelController.PlayerLevel.Value;
            var maxAllowedPieceLevelOnGrid = _piecesData.GetBiggestPieceLevelToLiveOnGrid(playerLevel);

            _bufferList.Clear();

            var tooBigToken = _gridMap.GetFirstTokenBiggerThanValueOrNull(maxAllowedPieceLevelOnGrid);
            if (tooBigToken == null)
            {
                return;
            }

            DestroyPiecesAround(tooBigToken);

            tooBigToken.Destroy();
            FireDestroyEffectForOvergrownPiece(tooBigToken);
            
            _signalBus.Fire(_overgrownExplosionSignal);
        }

        private void DestroyPiecesAround(IPiece tooBigToken)
        {
            _bufferList.Clear();
            var piecesAround = _gridMap.GetPiecesAroundPosition(tooBigToken.Position.Value, _bufferList);
            foreach (var piece in piecesAround)
            {
                piece.Destroy();

                FireDestroyEffectForPieceAround(piece);
            }
        }

        private void FireDestroyEffectForOvergrownPiece(IPiece tooBigToken)
        {
            var color = _piecesData.GetColorsSetForLevel(tooBigToken.Level.Value).InnerColor;

            var destroyParticles = _pieceDestroyOnOvergrownExplosionParticlesPool.Spawn();
            var viewPosition = _gridMap.GetViewPosition(tooBigToken.Position.Value);
            destroyParticles.Setup(color, viewPosition, DestroyParticlesSourceType.ExplodeOvergrown);
        }

        private void FireDestroyEffectForPieceAround(IPiece piece)
        {
            var destroyParticle = _pieceDestroyOnCombineParticlesPool.Spawn();
            var pos = _gridMap.GetViewPosition(piece.Position.Value);
            var col = _piecesData.GetColorsSetForLevel(piece.Level.Value).InnerColor;
            destroyParticle.Setup(col, pos, DestroyParticlesSourceType.Combine);
        }
    }
}