using UnityEngine;
using Zenject;

namespace Project.Scripts.Popups
{
    public abstract class PopupMaster : MonoBehaviour
    {
        [SerializeField] private PopupView _view = null;
        [Inject] protected readonly SignalBus SignalBus = null;

        private PopupActionSignal _popupActionSignal;

        protected abstract void SubscribeToRequest();
        protected abstract void UnsubscribeFromRequest();

        private void Awake()
        {
            _popupActionSignal = new PopupActionSignal(_view.GetType());
            AssertSerializedFields();
        }

        private void AssertSerializedFields()
        {
            Debug.Assert(_view, "Missing reference: _view", this);
        }

        protected virtual void OnRequest(IPopupRequest request)
        {
            _view.Show(request);
            _popupActionSignal.ActionType = PopupActionType.Show;
            SignalBus.Fire(_popupActionSignal);
        }

        protected void HideView()
        {
            _view.Hide();

            _popupActionSignal.ActionType = PopupActionType.Hide;
            SignalBus.Fire(_popupActionSignal);
        }
    }
}