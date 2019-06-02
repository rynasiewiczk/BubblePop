using UniRx;
using UnityEngine;

namespace Project.Aiming
{
    public interface IFindingCellToShootPieceController
    {
        ReactiveProperty<Vector2[]> PieceFlyPath { get; }
        Vector2Int PieceDestinationPosition { get; }
    }
}