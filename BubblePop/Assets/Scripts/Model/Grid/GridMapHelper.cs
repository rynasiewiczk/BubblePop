using System;
using System.Collections.Generic;
using Enums;
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
                        if (BubbleExistsAtPosition(gridMap, bubble.Position.Value + direction))
                        {
                            direction = new Vector2Int(-1, 0);
                        }
                    }

                    break;
                case BubbleSide.BottomRight:
                    direction = rowSideOfHit == GridRowSide.Left ? new Vector2Int(0, -1) : new Vector2Int(1, -1);
                    if (BubbleExistsAtPosition(gridMap, bubble.Position.Value + direction))
                    {
                        direction = direction.x == 0 ? new Vector2Int(-1, direction.y) : new Vector2Int(0, direction.y);
                        if (BubbleExistsAtPosition(gridMap, bubble.Position.Value + direction))
                        {
                            direction = new Vector2Int(1, 0);
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var result = position + direction;
            return result;
        }

        public static Vector2Int GetGridViewPositionVector2Int(this IGridMap gridMap, Vector2Int position)
        {
            var pos = GetGridViewPosition(gridMap, position);
            return new Vector2Int((int) pos.x, (int) pos.y);
        }

        public static Vector2 GetGridViewPosition(this IGridMap gridMap, Vector2Int position)
        {
            var offsetX = 0f;
            if (gridMap.GridRowSidesMap[position.y] == GridRowSide.Right)
            {
                offsetX += .5f;
            }

            var viewPositionInX = position.x + offsetX;
            var viewPositionInY = GetHeightOfRows(gridMap, position.y);
            return new Vector2(viewPositionInX, viewPositionInY);
        }

        public static List<IBubble> GetAllPlayableBubblesOnGrid(this IGridMap gridMap)
        {
            var list = new List<IBubble>();
            foreach (var bubble in gridMap.BubblesRegistry)
            {
                if (bubble.IsPlayable())
                {
                    list.Add(bubble);
                }
            }

            return list;
        }

        public static List<IBubble> GetAllBubblesOnGrid(this IGridMap gridMap)
        {
            var list = new List<IBubble>();
            foreach (var bubble in gridMap.BubblesRegistry)
            {
                if (bubble != null)
                {
                    list.Add(bubble);
                }
            }

            return list;
        }

        public static float GetHeightOfRows(this IGridMap gridMap,int rows)
        {
            var heightBetweenRows = Mathf.Pow(Mathf.Pow(DISTANCE_BETWEEN_BUBBLES, 2) - Mathf.Pow(HALF_OF_DISTANCE_BETWEEN_BUBBLES, 2), .5f);

            var result = heightBetweenRows * rows;
            return result;
        }

        public static bool BubbleExistsAtPosition(this IGridMap gridMap, Vector2Int position)
        {
            var bubble = gridMap.GetBubbleAtPositionOrNull(position);
            return bubble != null;
        }
    }
}