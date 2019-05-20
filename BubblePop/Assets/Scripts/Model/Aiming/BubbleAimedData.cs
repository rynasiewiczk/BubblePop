using Enums;
using Project.Bubbles;
using UnityEngine;

namespace Project.Aiming
{
    public class BubbleAimedData
    {
        public readonly IBubble Bubble;
        public readonly BubbleSide AimedSide;
        public readonly Vector2[] PathFromAimingPosition;

        public BubbleAimedData(IBubble bubble, BubbleSide aimedSide, Vector2[] pathFromAimingPosition)
        {
            Bubble = bubble;
            AimedSide = aimedSide;
            PathFromAimingPosition = pathFromAimingPosition;
        }
    }
}