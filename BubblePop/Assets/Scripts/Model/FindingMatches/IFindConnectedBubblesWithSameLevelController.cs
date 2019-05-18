using System.Collections.Generic;
using Project.Bubbles;
using UniRx;

namespace Model.FindingMatches
{
    public interface IFindConnectedBubblesWithSameLevelController
    {
        ReactiveProperty<List<IBubble>> BubblesToCombine { get; }
    }
}