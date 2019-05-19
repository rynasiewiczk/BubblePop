using System;
using UnityEngine;

namespace View
{
    [Serializable]
    public class BubbleViewSettings
    {
        public float OnHitBounceDistance = .15f;
        public float OnHidBounceHalfDuration = .17f;

        [Space] public float AppearScaleDuration = .16f;
    }
}