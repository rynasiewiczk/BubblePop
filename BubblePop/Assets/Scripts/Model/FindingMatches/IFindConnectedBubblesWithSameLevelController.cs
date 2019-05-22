using System.Collections.Generic;
using Project.Pieces;
using UniRx;

namespace Model.FindingMatches
{
    public interface IFindConnectedBubblesWithSameLevelController
    {
        ReactiveProperty<List<IBubble>> BubblesToCombine { get; }
    }
}