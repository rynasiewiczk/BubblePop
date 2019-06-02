using System;
using UnityEngine;

namespace Project.Scripts.Popups.AnimationTransitions
{
    public abstract class PopupAnimationTransition : MonoBehaviour
    {
        public abstract void AnimateShow(Transform target, Action onFinished);
        public abstract void AnimateHide(Transform target, Action onFinished);
    }
}