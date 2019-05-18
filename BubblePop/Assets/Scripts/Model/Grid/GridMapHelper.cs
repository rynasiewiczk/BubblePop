using System;
using System.Linq;
using Enums;
using Project.Aiming;
using Project.Bubbles;
using UnityEngine;

namespace Project.Grid
{
    public static class GridMapHelper
    {
        private const float DISTANCE_BETWEEN_BUBBLES = 1f;
        private const float HALF_OF_DISTANCE_BETWEEN_BUBBLES = DISTANCE_BETWEEN_BUBBLES / 2;

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
                    if (BubbleExistsAtPosition(gridMap, bubble.Position.Value + direction))
                    {
                        direction = direction.x == 0 ? new Vector2Int(1, direction.y) : new Vector2Int(0, direction.y);
                    }

                    break;
                case BubbleSide.BottomRight:
                    direction = rowSideOfHit == GridRowSide.Left ? new Vector2Int(0, -1) : new Vector2Int(1, -1);
                    if (BubbleExistsAtPosition(gridMap, bubble.Position.Value + direction))
                    {
                        direction = direction.x == 0 ? new Vector2Int(-1, direction.y) : new Vector2Int(0, direction.y);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var result = position + direction;
            return result;
        }

        public static Vector2 GetGridViewPosition(this IGridMap gridMap, Vector2Int position)
        {
            var offsetX = 0f;
            if (gridMap.GridRowSidesMap[position.y] == GridRowSide.Right)
            {
                offsetX += .5f;
            }

            var viewPositionInX = position.x + offsetX;
            var viewPositionInY = GetHeightOfRow(position.y);
            return new Vector2(viewPositionInX, viewPositionInY);
        }

        private static float GetHeightOfRow(int row)
        {
            var heightBetweenRows = Mathf.Pow(Mathf.Pow(DISTANCE_BETWEEN_BUBBLES, 2) - Mathf.Pow(HALF_OF_DISTANCE_BETWEEN_BUBBLES, 2), .5f);

            var result = heightBetweenRows * row;
            return result;
        }

        public static bool BubbleExistsAtPosition(this IGridMap gridMap, Vector2Int position)
        {
            var bubble = gridMap.GetBubbleAtPosition(position);
            return bubble != null;
        }
    }
}