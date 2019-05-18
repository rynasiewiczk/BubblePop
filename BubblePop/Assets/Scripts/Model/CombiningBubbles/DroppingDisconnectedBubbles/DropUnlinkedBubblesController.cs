using System.Collections.Generic;
using System.Linq;
using Enums;
using Project.Bubbles;
using Project.Grid;
using UniRx;
using UnityEngine;
using Zenject;

namespace Model.CombiningBubbles.DroppingDisconnectedBubbles
{
    public class DropUnlinkedBubblesController : IDropUnlinkedBubblesController
    {
        private IGridMap _gridMap = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly ICombineBubbles _combineBubbles = null;
        private readonly GridSettings _gridSettings = null;

        private List<IBubble> _allBubbles = new List<IBubble>();
        private List<IBubble> _bubblestToFall = new List<IBubble>();
        private List<IBubble> _bubblesToStay = new List<IBubble>();
        private List<IBubble> _topRowBubbles = new List<IBubble>();
        private List<IBubble> _allConnectedBubbles = new List<IBubble>();

        public DropUnlinkedBubblesController(IGridMap gridMap, IGameStateController gameStateController, ICombineBubbles combineBubbles,
            GridSettings gridSettings)
        {
            _gridMap = gridMap;
            _gridSettings = gridSettings;
            _gameStateController = gameStateController;
            _combineBubbles = combineBubbles;

            _gameStateController.GamePlayState.Where(x => x == GamePlayState.DropBubblesAfterCombining).Subscribe(x => DropUnlinkedBubbles());
        }

        private void DropUnlinkedBubbles()
        {
            _topRowBubbles.Clear();
            _topRowBubbles = _gridMap.GetAllTopPlayableRowBubblesOnGrid(_gridSettings, _topRowBubbles);
            _bubblestToFall.Clear();
            _bubblesToStay.Clear();

            foreach (var topBubble in _topRowBubbles)
            {
                if (_bubblesToStay.Contains(topBubble) || _bubblestToFall.Contains(topBubble))
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

                _bubblestToFall.Add(bubble);
            }

            for (int i = _bubblestToFall.Count - 1; i >= 0; i--)
            {
                if (_bubblestToFall[i].IsPlayable())
                {
                    _bubblestToFall[i].Destroy();
                }
            }

            var nextState = _combineBubbles.LastCombinedBubbleNeighboursWithSameLevelAmount <= 1 ? GamePlayState.Idle : GamePlayState.BubblesCombining;
            _gameStateController.ChangeGamePlayState(nextState);
        }
    }
}