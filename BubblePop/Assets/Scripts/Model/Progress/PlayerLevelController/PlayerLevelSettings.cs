using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Model.Progress.PlayerLevelController
{
    [Serializable] public class PlayerLevelSettings
    {
        private const int EXTRA_PERCENTAGE_TO_ADD_WHEN_SCORE_NOT_SPEFICIED = 200;
        [SerializeField] private List<int> _scoreRequirementsForLevels = null;

        public int GetRequirementForLevel(int level)
        {
            while (_scoreRequirementsForLevels.Count <= level)
            {
                _scoreRequirementsForLevels.Add(_scoreRequirementsForLevels.Last() +
                                                _scoreRequirementsForLevels.Last() * EXTRA_PERCENTAGE_TO_ADD_WHEN_SCORE_NOT_SPEFICIED / 100);
            }

            return _scoreRequirementsForLevels[level];
        }

        public int GetCurrentLevelByScore(int score)
        {
            var level = 0;
            for (int i = 0; i < _scoreRequirementsForLevels.Count; i++)
            {
                if (score >= _scoreRequirementsForLevels[i])
                {
                    level = i;
                }
                else
                {
                    break;
                }
            }

            return level;
        }

        public float GetCurrentLevelNormalizedProgress(int currentScore)
        {
            var currentLevel = GetCurrentLevelByScore(currentScore);
            var scoreForNextLevel = GetRequirementForLevel(currentLevel + 1);
            var scoreForCurrentLevel = GetRequirementForLevel(currentLevel);

            var scoreBetweenLevelsDifference = scoreForNextLevel - scoreForCurrentLevel;
            var scoreReachedInCurrentLevel = currentScore - scoreForCurrentLevel;

            var currentLevelProgressNormalized = (float) scoreReachedInCurrentLevel / scoreBetweenLevelsDifference;
            return currentLevelProgressNormalized;
        }
    }
}