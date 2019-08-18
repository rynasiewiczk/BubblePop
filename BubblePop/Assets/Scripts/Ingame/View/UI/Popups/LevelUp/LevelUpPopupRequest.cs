using System;
using Project.Scripts.Popups;

namespace Ingame.View.UI.Popups.LevelUp
{
    public class LevelUpPopupRequest : IPopupRequest
    {
        public int Level;
        public Action OnContinueButtonClick;
    }
}