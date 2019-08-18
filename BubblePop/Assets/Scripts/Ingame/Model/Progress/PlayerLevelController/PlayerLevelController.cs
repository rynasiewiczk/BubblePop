using Ingame.View.UI.Popups.LevelUp;
using Model.ScoreController;
using UniRx;
using Zenject;

namespace Model.Progress.PlayerLevelController
{
    public class PlayerLevelController : IPlayerLevelController
    {
        public ReactiveProperty<int> PlayerLevel { get; }

        private readonly PlayerLevelSettings _playerLevelSettings = null;
        private readonly SignalBus _signalBus = null;

        private readonly LevelUpPopupRequest _levelUpPopupRequest = new LevelUpPopupRequest();

        public PlayerLevelController(IScoreController scoreController, PlayerLevelSettings playerLevelSettings, SignalBus signalBus)
        {
            PlayerLevel = new ReactiveProperty<int>(1);
            _playerLevelSettings = playerLevelSettings;
            _signalBus = signalBus;

            scoreController.Score.Subscribe(UpdateLevelIfRequirementMet);
        }

        private void UpdateLevelIfRequirementMet(int score)
        {
            var reachedLevel = _playerLevelSettings.GetCurrentLevelByScore(score);
            if (reachedLevel > PlayerLevel.Value)
            {
                PlayerLevel.Value = reachedLevel;

                _levelUpPopupRequest.Level = PlayerLevel.Value;
                _signalBus.Fire(_levelUpPopupRequest);
            }
        }
    }
}