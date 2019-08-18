using System;
using UnityEngine;

namespace Model.Progress.PlayerLevelController
{
    [Serializable] public class PlayerLevelSettings
    {
        [SerializeField] private int _secondLevelScore = 17000;
        [SerializeField] private float _nextLevelMultiplier = 1.5f;

        public int GetRequirementForLevel(int level)
        {
            if (level < 1)
            {
                Debug.LogError("Provided level is less than minimal. Provided: " + level);
            }

            if (level == 1)
            {
                return 0;
            }

            if (level == 2)
            {
                return _secondLevelScore;
            }

            var scoreForLevel = _secondLevelScore;
            for (int i = 2; i < level; i++)
            {
                scoreForLevel = (int) (scoreForLevel * _nextLevelMultiplier);
            }

            return scoreForLevel;
        }

        public int GetCurrentLevelByScore(int score)
        {
            var level = 1;
            var scoreForLevel = 0;
            var scoreForNextLevel = _secondLevelScore;

            if (score >= scoreForLevel && score < scoreForNextLevel)
            {
                return level;
            }

            while (true)
            {
                level++;

                scoreForLevel = scoreForNextLevel;
                scoreForNextLevel = (int) (scoreForLevel * _nextLevelMultiplier);

                if (score >= scoreForLevel && score < scoreForNextLevel)
                {
                    return level;
                }
            }
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