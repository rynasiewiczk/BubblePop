using Project.Scripts.Popups.AnimationTransitions;
using UnityEngine;

namespace Project.Scripts.Popups
{
    [RequireComponent(typeof(PopupAnimationTransition))]
    public abstract class PopupView : MonoBehaviour
    {
        [SerializeField] private GameObject _inputBlocker = null;

        private PopupAnimationTransition _animationTransition = null;

        private PopupAnimationTransition PopupAnimationTransition
        {
            get
            {
                if (_animationTransition == null) _animationTransition = GetComponent<PopupAnimationTransition>();
                return _animationTransition;
            }
        }

        protected virtual void Awake()
        {
            Debug.Assert(PopupAnimationTransition, "Missing reference: _animationTransition", this);
            Debug.Assert(_inputBlocker, "Missing reference: _inputBlocker", this);
        }

        public virtual void Show(IPopupRequest request = null)
        {
            ClearButtonsListeners();
            SetInputBlocker(true);
            gameObject.SetActive(true);

            if (PopupAnimationTransition == null) return;
            PopupAnimationTransition.AnimateShow(transform, () => SetInputBlocker(false));
        }

        public virtual void Hide()
        {
            PopupAnimationTransition.AnimateHide(transform, () => gameObject.SetActive(false));
        }

        private void SetInputBlocker(bool enable)
        {
            if (_inputBlocker == null) return;
            _inputBlocker.SetActive(enable);
        }
        protected abstract void ClearButtonsListeners();
    }
}