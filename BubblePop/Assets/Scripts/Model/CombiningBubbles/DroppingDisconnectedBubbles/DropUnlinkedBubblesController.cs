using System.Collections.Generic;
using Enums;
using Project.Bubbles;
using Project.Grid;
using UniRx;
using Zenject;

namespace Model.CombiningBubbles.DroppingDisconnectedBubbles
{
    public class DropUnlinkedBubblesController : IDropUnlinkedBubblesController
    {
        private readonly IGridMap _gridMap = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly ICombineBubblesController _combineBubblesController = null;
        private readonly GridSettings _gridSettings = null;
        private readonly SignalBus _signalBus = null;
        
        private readonly DroppingUnlinkedBubbleSignal _droppingUnlinkedBubbleSignal = new DroppingUnlinkedBubbleSignal();

        private List<IBubble> _allBubbles = new List<IBubble>();
        private readonly List<IBubble> _bubblesToFall = new List<IBubble>();
        private readonly List<IBubble> _bubblesToStay = new List<IBubble>();
        private List<IBubble> _topRowBubbles = new List<IBubble>();
        private readonly List<IBubble> _allConnectedBubbles = new List<IBubble>();

        public DropUnlinkedBubblesController(IGridMap gridMap, IGameStateController gameStateController, ICombineBubblesController combineBubblesController,
            GridSettings gridSettings, SignalBus signalBus)
        {
            _gridMap = gridMap;
            _gridSettings = gridSettings;
            _gameStateController = gameStateController;
            _combineBubblesController = combineBubblesController;
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

            var nextState = _combineBubblesController.LastCombinedBubbleNeighboursWithSameLevelAmount <= 1 ? GamePlayState.ScrollRows : GamePlayState.BubblesCombining;
            _gameStateController.ChangeGamePlayState(nextState);
        }
    }
}