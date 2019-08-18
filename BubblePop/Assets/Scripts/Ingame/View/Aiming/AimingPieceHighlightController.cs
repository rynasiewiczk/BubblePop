using System.Collections.Generic;
using System.Linq;
using Enums;
using Model;
using Project.Grid;
using Project.Pieces;
using UniRx;
using Zenject;

namespace View.Aiming
{
    public class AimingPieceHighlightController : IAimingPieceHighlightController, ITickable
    {
        private readonly IGridMap _gridMap = null;

        private readonly AimingPieceHighlightParticlePool _pool;
        private readonly HashSet<AimingPieceHighlightParticle> _highlights = new HashSet<AimingPieceHighlightParticle>();

        private bool _aiming = false;
        private int _cachedLevel = -1;
        
        [Inject] private readonly INextBubbleLevelToSpawnController _nextBubbleLevelToSpawnController = null;

        public AimingPieceHighlightController(IGridMap gridMap, IGameStateController gameStateController,
            AimingPieceHighlightParticlePool highlightParticlePool)
        {
            _gridMap = gridMap;
            _pool = highlightParticlePool;

            gameStateController.GamePlayState.Where(x => x == GamePlayState.Aiming).Subscribe(x => _aiming = true);
            gameStateController.GamePlayState.Where(x => x != GamePlayState.Aiming).Subscribe(x =>
            {
                _cachedLevel = -1;
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

            var level = _nextBubbleLevelToSpawnController.NextBubbleLevelToSpawn.Value;
            
            
            if (level == _cachedLevel)
            {
                return;
            }

            _cachedLevel = level;

            DespawnList();

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
            foreach (var highlight in _highlights)
            {
                _pool.Despawn(highlight);
            }
            _highlights.Clear();
        }
    }
}