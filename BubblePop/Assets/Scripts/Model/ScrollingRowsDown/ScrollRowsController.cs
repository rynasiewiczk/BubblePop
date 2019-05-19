using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Project.Bubbles;
using Project.Grid;
using UniRx;
using UnityEngine;
using Zenject;

namespace Model.ScrollingRowsDown
{
    public class ScrollRowsController : IScrollRowsController
    {
        private IGameStateController _gameStateController = null;
        private readonly IGridMap _gridMap = null;
        private readonly GridSettings _gridSettings = null;
        private readonly SignalBus _signalBus = null;
        private ScrollRowsSignal _scrollRowsSignal = new ScrollRowsSignal();

        public ScrollRowsController(IGameStateController gameStateController, IGridMap gridMap, SignalBus signalBus, GridSettings gridSettings)
        {
            _gameStateController = gameStateController;
            _gridMap = gridMap;
            _signalBus = signalBus;
            _gridSettings = gridSettings;
            gameStateController.GamePlayState.Where(x => x == GamePlayState.ScrollRows).Subscribe(x => ScrollRowsIfNeeded());
        }

        private void ScrollRowsIfNeeded()
        {
            var allTokens = _gridMap.GetAllBubblesOnGrid();

            var lowestToken = _gridMap.GetLowestRowWithBubble(allTokens);
            var bottomEdge = _gridSettings.AlwasFreeBottomLines;
            var topEdge = _gridSettings.RowToScrollTokensDown;

            if (lowestToken >= bottomEdge && lowestToken < topEdge)
            {
                _gameStateController.ChangeGamePlayState(GamePlayState.Idle);
                return;
            }

            if (lowestToken < bottomEdge)
            {
                ScrollRowsUp(allTokens);

                FireScrollRowsSignal(1);

                DOVirtual.DelayedCall(_gridSettings.ScrollOneRowDuration, () => _gameStateController.ChangeGamePlayState(GamePlayState.Idle));
                return;
            }

            if (lowestToken >= topEdge)
            {
                var rowsToScroll = lowestToken - (topEdge + 1);
                ScrollRowsDown(allTokens, rowsToScroll);

                FireScrollRowsSignal(rowsToScroll);

                DOVirtual.DelayedCall(_gridSettings.ScrollOneRowDuration * Mathf.Abs(rowsToScroll),
                    () => _gameStateController.ChangeGamePlayState(GamePlayState.Idle));
            }
        }

        private void FireScrollRowsSignal(int rowsToScroll)
        {
            _scrollRowsSignal.RowsToScroll = rowsToScroll;
            _signalBus.Fire(_scrollRowsSignal);
        }

        private void ScrollRowsUp(List<IBubble> list)
        {
            list = list.OrderBy(x => x.Position.Value.y).ToList();

            _gridMap.SwitchRowSidesOnMap(1);

            for (int x = list.Count - 1; x >= 0; x--)
            {
                var bubble = list[x];
                var cellUp = _gridMap.GetCellAtPositionOrNull(bubble.Position.Value);
                if (cellUp != null)
                {
                    bubble.MoveUpOneCell();
                }
                else
                {
                    if (!bubble.Destroyed.IsDisposed)
                    {
                        bubble.Destroy();
                    }
                }
            }
        }

        private void ScrollRowsDown(List<IBubble> list, int rowsToScroll)
        {
            list = list.OrderByDescending(x => x.Position.Value.y).ToList();

            _gridMap.SwitchRowSidesOnMap(rowsToScroll);

            for (int x = list.Count - 1; x >= 0; x--)
            {
                var bubble = list[x];
                var cellUp = _gridMap.GetCellAtPositionOrNull(bubble.Position.Value);
                if (cellUp != null)
                {
                    bubble.MoveDown(rowsToScroll);
                }
                else
                {
                    bubble.Destroy();
                }
            }
        }
    }
}