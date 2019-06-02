using DG.Tweening;
using Model.ScoreController;
using UniRx;
using View.UI;

public class ProgressBarParticlesController : IProgressBarParticlesController
{
    private readonly LevelProgressBarDisplay _levelProgressBarDisplay = null;
    private readonly ProgressBarParticlesPool _progressBarParticlesPool = null;

    public ProgressBarParticlesController(IScoreController scoreController, LevelProgressBarDisplay levelProgressBarDisplay,
        ProgressBarParticlesPool progressBarParticlesPool)
    {
        _levelProgressBarDisplay = levelProgressBarDisplay;
        _progressBarParticlesPool = progressBarParticlesPool;

        scoreController.Score.Skip(1).Subscribe(x => SpawnParticles(x - scoreController.LastGainedScore));
    }

    private void SpawnParticles(int gainedScore)
    {
        //delay to get the position after the update
        DOVirtual.DelayedCall(_levelProgressBarDisplay.FillDuration, () =>
        {
            var spawnPosition = _levelProgressBarDisplay.GetProgressBarRightEdge();
            var particles = _progressBarParticlesPool.Spawn();
            particles.Setup(spawnPosition, gainedScore);
        });
    }
}