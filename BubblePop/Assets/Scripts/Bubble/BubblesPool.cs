using Zenject;

namespace Project.Bubbles
{
    public class BubblesPool : MemoryPool<BubbleData, Bubble>
    {
        private BubbleData _bubbleData = null;

        public BubblesPool(BubbleData bubbleData)
        {
            _bubbleData = bubbleData;
        }
    }
}