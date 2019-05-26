using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Project.Pieces;
using Project.Grid;
using UniRx;

namespace Model.ScrollingRowsDown
{
    public class ScrollRowsController : IScrollRowsController
    {
        private readonly IGameStateController _gameStateController = null;
        private readonly IGridMap _gridMap = null;
        private readonly GridSettings _gridSettings = null;
        
        public ReactiveCommand<int> RowsScrolled { get; }

        public ScrollRowsController(IGameStateController gameStateController, IGridMap gridMap, GridSettings gridSettings)
        {
            RowsScrolled = new ReactiveCommand<int>();
            
            _gameStateController = gameStateController;
            _gridMap = gridMap;
            _gridSettings = gridSettings;
            _gameStateController.GamePlayState.Where(x => x == GamePlayState.ScrollRows).Subscribe(x => ScrollRowsIfNeeded());
        }

        private void ScrollRowsIfNeeded()
        {
            var allTokens = _gridMap.GetAllBubblesOnGrid();

            var lowestToken = BubblesFindingHelper.GetLowestRowWithBubble(allTokens);
            var bottomEdge = _gridSettings.AlwasFreeBottomLines;
            var topEdge = _gridSettings.RowToScrollTokensDown;

            if (lowestToken >= bottomEdge && lowestToken < topEdge)
            {
                _gameStateController.ChangeGamePlayState(GamePlayState.FillBubblesAboveTop);
                return;
            }

            if (lowestToken < bottomEdge)
            {
                ScrollRowsUp(allTokens);

                ExecuteRowScrolledCommand(1);

                DOVirtual.DelayedCall(_gridSettings.ScrollOneRowDuration, () => _gameStateController.ChangeGamePlayState(GamePlayState.FillBubblesAboveTop), false);
                return;
            }

            if (lowestToken >= topEdge)
            {
                ScrollRowsDown(allTokens);

                ExecuteRowScrolledCommand(-1);

                DOVirtual.DelayedCall(_gridSettings.ScrollOneRowDuration,
                    () => _gameStateController.ChangeGamePlayState(GamePlayState.FillBubblesAboveTop), false);
            }
        }

        private void ExecuteRowScrolledCommand(int rowsToScroll)
        {
            RowsScrolled.Execute(rowsToScroll);
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

        private void ScrollRowsDown(List<IBubble> list)
        {
            list = list.OrderByDescending(x => x.Position.Value.y).ToList();

            _gridMap.SwitchRowSidesOnMap(1);

            for (int x = list.Count - 1; x >= 0; x--)
            {
                var bubble = list[x];
                var cellUp = _gridMap.GetCellAtPositionOrNull(bubble.Position.Value);
                if (cellUp != null)
                {
                    bubble.MoveDownOneCell();
                }
                else
                {
                    bubble.Destroy();
                }
            }
        }

    }
}
