using System.Collections.Generic;
using Project.Pieces;
using UniRx;

namespace Model.FindingMatches
{
    public interface IFindConnectedPiecesWithSameLevelController
    {
        ReactiveCommand<List<IBubble>> CombinePieces { get; }
    }
}