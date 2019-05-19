using Model.ScoreController;
using UniRx;
using UnityEngine;

namespace Model.Progress.PlayerLevelController
{
    public class PlayerLevelController : IPlayerLevelController
    {
        public ReactiveProperty<int> PlayerLevel { get; }

        private readonly PlayerLevelSettings _playerLevelSettings = null;

        public PlayerLevelController(IScoreController scoreController, PlayerLevelSettings playerLevelSettings)
        {
            PlayerLevel = new ReactiveProperty<int>(1);
            _playerLevelSettings = playerLevelSettings;
            
            scoreController.Score.Subscribe(UpdateLevelIfRequirementMet);
        }

        private void UpdateLevelIfRequirementMet(int score)
        {
            var reachedLevel = _playerLevelSettings.GetCurrentLevelByScore(score);
            if (reachedLevel > PlayerLevel.Value)
            {
                PlayerLevel.Value = reachedLevel;
            }
        }
    }
}