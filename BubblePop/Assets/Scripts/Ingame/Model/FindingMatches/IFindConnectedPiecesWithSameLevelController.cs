using System.Collections.Generic;
using Project.Pieces;
using UniRx;

namespace Model.FindingMatches
{
    public interface IFindConnectedPiecesWithSameLevelController
    {
        ReactiveCommand<List<IPiece>> CombinePieces { get; }
        ReactiveProperty<int> CombinationsInRow { get; }
    }
}