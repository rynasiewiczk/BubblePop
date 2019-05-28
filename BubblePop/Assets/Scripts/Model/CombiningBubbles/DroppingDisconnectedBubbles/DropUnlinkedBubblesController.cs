using System.Collections.Generic;
using Enums;
using Project.Pieces;
using Project.Grid;
using UniRx;
using Zenject;

namespace Model.CombiningBubbles.DroppingDisconnectedBubbles
{
    public class DropUnlinkedBubblesController : IDropUnlinkedBubblesController
    {
        private readonly IGridMap _gridMap = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly ICombinePiecesController _combinePiecesController = null;
        private readonly GridSettings _gridSettings = null;
        private readonly SignalBus _signalBus = null;
        
        private readonly DroppingUnlinkedBubbleSignal _droppingUnlinkedBubbleSignal = new DroppingUnlinkedBubbleSignal();

        private List<IPiece> _allBubbles = new List<IPiece>();
        private readonly List<IPiece> _bubblesToFall = new List<IPiece>();
        private readonly List<IPiece> _bubblesToStay = new List<IPiece>();
        private List<IPiece> _topRowBubbles = new List<IPiece>();
        private readonly List<IPiece> _allConnectedBubbles = new List<IPiece>();

        public DropUnlinkedBubblesController(IGridMap gridMap, IGameStateController gameStateController, ICombinePiecesController combinePiecesController,
            GridSettings gridSettings, SignalBus signalBus)
        {
            _gridMap = gridMap;
            _gridSettings = gridSettings;
            _gameStateController = gameStateController;
            _combinePiecesController = combinePiecesController;
            _signalBus = signalBus;

            _gameStateController.GamePlayState.Where(x => x == GamePlayState.DropBubblesAfterCombining).Subscribe(x => DropUnlinkedBubbles());
        }

        private void DropUnlinkedBubbles()
        {
            _topRowBubbles.Clear();
            _topRowBubbles = _gridMap.GetAllTopPlayableRowBubblesOnGrid(_gridSettings, _topRowBubbles);
            _bubblesToFall.Clear();
            _bubblesToStay.Clear();

            foreach (var topBubble in _topRowBubbles)
            {
                if (_bubblesToStay.Contains(topBubble) || _bubblesToFall.Contains(topBubble))
                {
                    continue;
                }

                _gridMap.GetAllConnectedBubbles(topBubble, _bubblesToStay, _allConnectedBubbles);
            }

            _allBubbles.Clear();
            _allBubbles = _gridMap.GetAllPlayableBubblesOnGrid();
            foreach (var bubble in _allBubbles)
            {
                if (_bubblesToStay.Contains(bubble))
                {
                    continue;
                }

                _bubblesToFall.Add(bubble);
            }

            for (int i = _bubblesToFall.Count - 1; i >= 0; i--)
            {
                if (!_gridMap.IsBubblePlayable(_bubblesToFall[i]))
                {
                    continue;
                }
                _droppingUnlinkedBubbleSignal.Position = _bubblesToFall[i].Position.Value;
                _droppingUnlinkedBubbleSignal.Level = _bubblesToFall[i].Level.Value;
                _signalBus.Fire(_droppingUnlinkedBubbleSignal);
                    
                _bubblesToFall[i].Destroy();
            }

            var nextState = _combinePiecesController.LastCombinedBubbleNeighboursWithSameLevelAmount <= 1 ? GamePlayState.ScrollRows : GamePlayState.BubblesCombining;
            _gameStateController.ChangeGamePlayState(nextState);
        }
    }
}