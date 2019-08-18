using System.Collections.Generic;
using System.Linq;
using Enums;
using Model;
using Project.Aiming;
using Project.Grid;
using Project.Pieces;
using Sirenix.Utilities;
using UniRx;
using Zenject;

namespace View.Aiming
{
    public class AimingPieceHighlightController : IAimingPieceHighlightController, ITickable
    {
        private readonly IGridMap _gridMap = null;
        private readonly IAimEndPointFinder _aimEndPointFinder = null;

        private readonly AimingPieceHighlightParticlePool _pool;

        private readonly HashSet<AimingPieceHighlightParticle> _highlights = new HashSet<AimingPieceHighlightParticle>();

        private bool _aiming = false;
        private IPiece _cachedAimingPiece = null;

        public AimingPieceHighlightController(IGridMap gridMap, IAimEndPointFinder aimEndPointFinder, IGameStateController gameStateController,
            AimingPieceHighlightParticlePool highlightParticlePool)
        {
            _gridMap = gridMap;
            _aimEndPointFinder = aimEndPointFinder;
            _pool = highlightParticlePool;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.Aiming).Subscribe(x => _aiming = true);
            gameStateController.GamePlayState.Where(x => x != GamePlayState.Aiming).Subscribe(x =>
            {
                _aiming = false;
                DespawnList();
            });
        }

        public void Tick()
        {
            if (!_aiming)
            {
                return;
            }

            var aimPiece = _aimEndPointFinder.AimedPieceData;
            if (aimPiece == null)
            {
                DespawnList();
                return;
            }

            if (aimPiece.Piece == _cachedAimingPiece)
            {
                return;
            }

            _cachedAimingPiece = aimPiece.Piece;

            DespawnList();

            var level = aimPiece.Piece.Level.Value;
            var closePiecesWithLevel = _gridMap.GetAllBubblesOnGrid().Where(x => x.Level.Value == level && _gridMap.IsPiecePlayable(x));
            foreach (var piece in closePiecesWithLevel)
            {
                var pos = piece.Position.Value;
                var viewPosition = _gridMap.GetViewPosition(pos);

                var highlight = _pool.Spawn();
                highlight.transform.position = viewPosition;
                _highlights.Add(highlight);
            }
        }

        private void DespawnList()
        {
            _highlights.ForEach(x => _pool.Despawn(x));
            _highlights.Clear();
        }
    }
}