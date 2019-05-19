using System;
using UnityEngine;
using UnityEngine.Serialization;

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

        [Space] public float SmallPrewarmBubbleSize = .50f;
        public float SmallPrewarmBubbleAppearDuration = .16f;

        public float AimingBubbleOverscale = 1.1f;
        public float AimingBubbleFromOverscaleToNormalDuration = .3f;
        public float AimingBubbleTransitionDuration = .16f;

[Space] public float DropBubbleMinStartVerticalVelocity = 4f;

        [FormerlySerializedAs("DropBubbleStartVerticalVelocity")] public float DropBubbleMaxStartVerticalVelocity = 2f;
        [FormerlySerializedAs("DropBubbleHorizontalVelocity")] public float DropBubbleMaxHorizontalVelocity = .5f;
        public float DropBubbleGravity = -14f;
        public float DropBubbleRotationSpeed = 8f;
        public float DropBubbleScaleReduceSpeed = .2f;
        public float DropBubbleTransparencyLossSpeed = .2f;
        public float DroppBubbleHeightToDespawn = -3f;
    }
}