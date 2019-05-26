using System.Collections.Generic;
using Project.Pieces;
using UniRx;

namespace Model.FindingMatches
{
    public interface IFindConnectedPiecesWithSameLevelController
    {
        ReactiveProperty<List<IBubble>> PiecesToCombine { get; }
    }
}