using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Project.Scripts.Popups
{
    public abstract class PopupsRegistry : MonoBehaviour, IPopupsRegistry
    {
        [Inject] private readonly SignalBus _signalBus = null;

        private List<PopupView> _popupsMap;
        private readonly Stack<PopupView> _openedPopups = new Stack<PopupView>();
        private List<KeyValuePair<Type, PopupActionType>> _popupsHistory = new List<KeyValuePair<Type, PopupActionType>>();

        private void Start()
        {
            _popupsMap = transform.GetComponentsInChildren<PopupView>(true).ToList();
            _signalBus.Subscribe<PopupActionSignal>(RegisterPopupAction);
        }

        private void RegisterPopupAction(PopupActionSignal signal)
        {
            if (signal.ActionType == PopupActionType.Show)
            {
                RegisterPopupOnShown(signal.PopupType, signal.ActionType);
            }
        }

        private void RegisterPopupOnShown(Type popupType, PopupActionType actionType)
        {
            var popup = _popupsMap.FirstOrDefault(x => x.GetType() == popupType);
            if (popup == null)
            {
                Debug.LogError("Popup of given type is not in the registry! Popup type: " + popupType, this);
            }

            _openedPopups.Push(popup);
            _popupsHistory.Add(new KeyValuePair<Type, PopupActionType>(popupType, actionType));
        }
    }
}