using System;
using UnityEngine;

namespace View
{
    [Serializable] public class BubbleViewSettings
    {
        public const int RENDER_LAYERS_MULTIPLAYER = 10;
        public const int TEXT_RENDER_LAYER_ADDITION = 1;

        public float OnHitBounceDistance = .15f;
        public float OnHidBounceHalfDuration = .17f;

        [Space] public float AppearScaleDuration = .16f;
        public AnimationCurve AppearFromTopScaleEase;

        [Space] public float SmallPrewarmBubbleSize = .50f;
        public float SmallPrewarmBubbleAppearDuration = .16f;

        public float AimingBubbleOverscale = 1.1f;
        public float AimingBubbleFromOverscaleToNormalDuration = .3f;
        public float AimingBubbleTransitionDuration = .16f;

        [Space] public float DropBubbleMinStartVerticalVelocity = 4f;

        public float DropBubbleMaxHorizontalVelocity = .5f;
        public float DropBubbleMaxStartVerticalVelocity = 2f;
        public float DropBubbleGravity = -14f;
        public float DropBubbleRotationSpeed = 8f;
        public float DropBubbleScaleReduceSpeed = .2f;
        public float DropBubbleTransparencyLossSpeed = .2f;
        public float DroppBubbleHeightToDespawn = -3f;

        [Space] public Vector2Int DestroyOnCombineParticlesAmountRange = new Vector2Int(3, 6);
        public Vector2Int DestroyOnDropParticlesAmountRange = new Vector2Int(5, 8);
        public Vector2Int DestroyOnOvergrownExplosionParticlesAmountRange = new Vector2Int(30, 40);
    }
}