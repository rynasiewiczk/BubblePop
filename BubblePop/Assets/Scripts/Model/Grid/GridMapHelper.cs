using System;
using Enums;
using Project.Aiming;
using Project.Bubbles;
using UnityEngine;

namespace Project.Grid
{
    public static class GridMapHelper
    {
        public static Vector2Int GetPositionToSpawnBubble(this IGridMap gridMap, IBubble bubble, BubbleSide aimedSide)
        {
            var position = bubble.Position.Value;
            Vector2Int direction;

            var rowSideOfHit = gridMap.GridRowSidesMap[position.y];

            switch (aimedSide)
            {
                case BubbleSide.TopLeft:
                    direction = new Vector2Int(-1, 0);
                    break;
                case BubbleSide.TopRight:
                    direction = new Vector2Int(1, 0);
                    break;
                case BubbleSide.BottomLeft:
                    direction = rowSideOfHit == GridRowSide.Left ? new Vector2Int(-1, -1) : new Vector2Int(0, -1);
                    break;
                case BubbleSide.BottomRight:
                    direction = rowSideOfHit == GridRowSide.Left ? new Vector2Int(0, -1) : new Vector2Int(1, -1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var result = position + direction;
            return result;
        }
    }
}