using UniRx;

namespace Model.ScoreController
{
    public interface IScoreController
    {
        ReactiveProperty<int> Score { get; }
        int LastGainedScore { get; }
    }
}