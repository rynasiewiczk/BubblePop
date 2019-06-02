using System;

namespace Project.Scripts.Popups
{
    public class PopupActionSignal
    {
        public Type PopupType;
        public PopupActionType ActionType;

        public PopupActionSignal(Type popupType)
        {
            PopupType = popupType;
        }
    }
}