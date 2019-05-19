using System;
using UnityEngine;

namespace View
{
    [Serializable]
    public class BubbleViewSettings
    {
        public const int RENDER_LAYERS_MULTIPLAYER = 10;
        public const int TEXT_RENDER_LAYER_ADDITION = 1;
        
        public float OnHitBounceDistance = .15f;
        public float OnHidBounceHalfDuration = .17f;

        [Space] public float AppearScaleDuration = .16f;
    }
}