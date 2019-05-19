using System.Collections.Generic;
using UnityEngine;

namespace Project.Aiming
{
    public interface IBubbleDestinationFinder
    {
        BubbleAimedData AimedBubbleData { get; }
        List<Vector2> AimPath { get; }
    }
}