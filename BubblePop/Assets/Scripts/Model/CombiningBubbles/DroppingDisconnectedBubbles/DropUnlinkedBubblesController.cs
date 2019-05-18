using System.Linq;
using Enums;
using Project.Grid;
using UniRx;

namespace Model.CombiningBubbles.DroppingDisconnectedBubbles
{
    public class DropUnlinkedBubblesController : IDropUnlinkedBubblesController
    {
        private IGridMap _gridMap = null;
        private readonly IGameStateController _gameStateController = null;
        private readonly ICombineBubbles _combineBubbles = null;

        public DropUnlinkedBubblesController(IGridMap gridMap, IGameStateController gameStateController, ICombineBubbles combineBubbles)
        {
            _gridMap = gridMap;
            _gameStateController = gameStateController;
            _combineBubbles = combineBubbles;

            _gameStateController.GamePlayState.Where(x => x == GamePlayState.DropBubblesAfterCombining).Subscribe(x => DropUnlinkedBubbles());
        }

        private void DropUnlinkedBubbles()
        {
            var bubbles = _gridMap.GetAllPlayableBubblesOnGrid();

            bubbles = bubbles.OrderBy(x => x.Position.Value.y).ToList();

            for (int i = bubbles.Count - 1; i >= 0; i--)
            {
                var hasConnectionFromTop = _gridMap.HasBubbleConnectionFromTop(bubbles[i]);
                if (!hasConnectionFromTop)
                {
                    bubbles[i].Destroy();
                }
            }

            var nextState = _combineBubbles.LastCombinedBubbleNeighboursWithSameLevelAmount <= 1 ? GamePlayState.Idle : GamePlayState.BubblesCombining;
            _gameStateController.ChangeGamePlayState(nextState);
        }
    }
}