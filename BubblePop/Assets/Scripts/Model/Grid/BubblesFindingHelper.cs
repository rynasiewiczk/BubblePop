using System.Collections.Generic;
using Project.Bubbles;
using Project.Bubbles.PlacingOnGrid;
using UnityEngine;

namespace Project.Grid
{
    public static class BubblesFindingHelper
    {
        private static readonly Vector2Int[] _leftRowCheckVectors = new Vector2Int[]
        {
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, -1),

            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
        };

        private static readonly Vector2Int[] _rightRowCheckVectors = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),

            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
        };

        public static List<IBubble> FindBubblesToCollapse(this IGridMap map, IBubble startBubble, List<IBubble> bufferList)
        {
            bufferList.Add(startBubble);
            var bubbleLevel = startBubble.Level.Value;

            GetNeighboursWithLevel(map, bubbleLevel, startBubble.Position.Value, Vector2Int.zero, bufferList);

            return bufferList;
        }

        public static List<IBubble> FindBubblesToCollapse(this IGridMap gridMap, int level, Vector2Int position, List<IBubble> bufferList)
        {
            var bubbleAtPosition = gridMap.GetBubbleAtPositionOrNull(position);
            if (bubbleAtPosition != null)
            {
                bufferList.Add(bubbleAtPosition);
            }

            GetNeighboursWithLevel(gridMap, level, position, Vector2Int.zero, bufferList);
            return bufferList;
        }

        private static void GetNeighboursWithLevel(IGridMap map, int level, Vector2Int startPos, Vector2Int dirToIgnore, List<IBubble> bufferList)
        {
            var rowSide = map.GetGridSideForRow(startPos.y);
            var checkVectors = rowSide == GridRowSide.Left ? _leftRowCheckVectors : _rightRowCheckVectors;

            foreach (var checkVector in checkVectors)
            {
                AddToListIfMatch(map, level, startPos, checkVector, dirToIgnore, bufferList);
            }
        }

        private static void AddToListIfMatch(IGridMap map, int level, Vector2Int startPos, Vector2Int dirToCheck, Vector2Int dirToIgnore,
            List<IBubble> bufferList)
        {
            var oppositeDir = dirToCheck * -1;
            var bubbleAtPosition = map.GetBubbleAtPositionOrNull(startPos + dirToCheck);

            if (dirToCheck == dirToIgnore || bubbleAtPosition == null || bufferList.Contains(bubbleAtPosition))
            {
                return;
            }

            if (!BubbleIsMatch(bubbleAtPosition, map.GetCellAtPositionOrNull(startPos + dirToCheck), level))
            {
                return;
            }

            bufferList.Add(bubbleAtPosition);
            GetNeighboursWithLevel(map, level, startPos + dirToCheck, oppositeDir, bufferList);
        }

        private static bool BubbleIsMatch(IBubble bubble, ICell cellAtPosition, int level)
        {
            return bubble != null && bubble.Level.Value == level;
        }

        public static bool HasBubbleConnectionFromTop(this IGridMap gridMap, IBubble bubble)
        {
            IBubble bubbleOnTop = null;

            var rowSide = gridMap.GetGridSideForRow(bubble.Position.Value.y);

            var posToCheck = bubble.Position.Value;
            posToCheck += rowSide == GridRowSide.Left ? new Vector2Int(-1, 1) : new Vector2Int(0, 1);
            bubbleOnTop = gridMap.GetBubbleAtPositionOrNull(posToCheck);
            if (bubbleOnTop != null && bubble.IsPlayable())
            {
                return true;
            }

            posToCheck = bubble.Position.Value;
            posToCheck += rowSide == GridRowSide.Left ? new Vector2Int(0, 1) : new Vector2Int(1, 1);
            bubbleOnTop = gridMap.GetBubbleAtPositionOrNull(posToCheck);
            if (bubbleOnTop != null && bubble.IsPlayable())
            {
                return true;
            }

            return false;
        }
    }
}