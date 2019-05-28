using UniRx;
using UnityEngine;

namespace Model.CombiningBubbles
{
    public interface ICombinePiecesController
    {
        int LastCombinedPieceNeighboursWithSameLevelAmount { get; }
        ReactiveProperty<Vector2Int> PositionOfCollapse { get; }
    }
}