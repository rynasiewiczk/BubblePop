using System;
using UnityEngine;

namespace Project.Scripts.Popups.AnimationTransitions
{
    [Serializable]
    public class PopupAnimationDefaultParameters
    {
        public float TransitionDuration = .6f;
        public float StartScale = .75f;

        public AnimationCurve Easing;
    }
}