using UniRx;
using UnityEngine;

namespace Model.CombiningBubbles
{
    public interface ICombinePiecesController
    {
        int LastCombinedBubbleNeighboursWithSameLevelAmount { get; }
        ReactiveProperty<Vector2Int> PositionOfCollapse { get; }
    }
}