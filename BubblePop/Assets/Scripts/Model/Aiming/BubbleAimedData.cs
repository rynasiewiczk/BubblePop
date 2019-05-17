using Enums;
using Project.Bubbles;
using UnityEngine;

namespace Project.Aiming
{
    public class BubbleAimedData
    {
        public IBubble Bubble;
        public BubbleSide AimedSide;
        public Vector2[] Path;

        public BubbleAimedData(IBubble bubble, BubbleSide aimedSide, Vector2[] path)
        {
            Bubble = bubble;
            AimedSide = aimedSide;
            Path = path;
        }
    }
}