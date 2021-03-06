using System.Collections.Generic;
using Enums;
using Model.CombiningBubbles;
using Model.FindingMatches;
using Project.Grid;
using Project.Pieces;
using UniRx;
using UnityEngine;

namespace View.DestroyParticles
{
    public class PieceDestroyOnCombineParticlesController : IPieceDestroyOnCombineParticlesController
    {
        private readonly PieceDestroyOnCombineParticlesPool _pieceDestroyOnCombineParticlesPool = null;
        private readonly IGridMap _gridMap = null;

        private List<IPiece> _combinedPieces = new List<IPiece>();

        public PieceDestroyOnCombineParticlesController(IFindConnectedPiecesWithSameLevelController connectedPiecesWithSameLevelController,
            ICombinePiecesController combinePiecesController, PieceDestroyOnCombineParticlesPool pieceDestroyOnCombineParticlesPool, IGridMap gridMap)
        {
            _pieceDestroyOnCombineParticlesPool = pieceDestroyOnCombineParticlesPool;
            _gridMap = gridMap;

            combinePiecesController.PositionOfCollapse.Skip(1).Subscribe(SpawnDestroyElement);
            connectedPiecesWithSameLevelController.CombinePieces.Where(x => x.Count > 0).Subscribe(x => _combinedPieces = x);
        }

        private void SpawnDestroyElement(Vector2Int combinePosition)
        {
            foreach (var piece in _combinedPieces)
            {
                if (piece.Position.Value == combinePosition)
                {
                    continue;
                }

                var position = _gridMap.GetViewPosition(piece.Position.Value);
                var effect = _pieceDestroyOnCombineParticlesPool.Spawn();
                effect.Setup(piece.Level.Value, position, DestroyParticlesSourceType.Combine);
            }
        }
    }
}