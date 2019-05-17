using Enums;
using Project.Bubbles;

namespace DefaultNamespace
{
    public class BubbleAimedData
    {
        public IBubble Bubble;
        public BubbleSide AimedSide;

        public BubbleAimedData(IBubble bubble, BubbleSide aimedSide)
        {
            Bubble = bubble;
            AimedSide = aimedSide;
        }
    }
}