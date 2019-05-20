using UniRx;
using UnityEngine;

namespace Project.Aiming
{
    public interface IFindingCellToShootBubbleController
    {
        ReactiveProperty<Vector2[]> BubbleFlyPath { get; }
        Vector2Int BubbleDestination { get; }
    }
}