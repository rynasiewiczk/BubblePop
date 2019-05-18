using DG.Tweening;
using Enums;
using UniRx;

namespace Model.CombiningBubbles.DroppingDisconnectedBubbles
{
    public class WaitAfterCombiningController : IWaitAfterCombiningController
    {
        public WaitAfterCombiningController(IGameStateController gameStateController, BubbleData bubbleData)
        {
            gameStateController.GamePlayState.Where(x => x == GamePlayState.WaitingForBubblesCombine).Subscribe(x =>
            {
                DOVirtual.DelayedCall(bubbleData.CombiningDuration,
                    () => gameStateController.ChangeGamePlayState(GamePlayState.DropBubblesAfterCombining));
            });
        }
    }
}