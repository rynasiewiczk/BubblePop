using Enums;
using Model;
using Project.Grid;
using UniRx;
using UnityEngine;

namespace Ingame.Model.ExplodingAfterCombining
{
    public class ExplodingTooBigPiecesController : IExplodingTooBigPiecesController
    {
        private IGridMap _gridMap = null;
        private PiecesData _piecesData = null;

        public ExplodingTooBigPiecesController(IGameStateController gameStateController, IGridMap gridMap, PiecesData piecesData)
        {
            _gridMap = gridMap;
            _piecesData = piecesData;
            
            gameStateController.GamePlayState.Where(x => x == GamePlayState.DropAndExplodePiecesAfterCombining).Subscribe(x => ExplodeTooBigPieces());
        }

        private void ExplodeTooBigPieces()
        {
            //var tooBigTokens = _gridMap.GetTooBigTokens();
        }
    }
}