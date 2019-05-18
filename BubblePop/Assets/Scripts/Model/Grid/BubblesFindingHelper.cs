using System.Collections.Generic;
using Project.Bubbles;
using UnityEngine;

namespace Project.Grid
{
    public static class BubblesFindingHelper
    {
        public static List<IBubble> FindBubblesToCollapse(this IGridMap map, IBubble startBubble, List<IBubble> bufferList)
        {
            bufferList.Add(startBubble);
            var bubbleLevel = startBubble.Level.Value;

            GetNeighboursWithLevel(map, bubbleLevel, startBubble.Position.Value, Vector2Int.zero, bufferList);

            return bufferList;
        }

        private static void GetNeighboursWithLevel(IGridMap map, int level, Vector2Int startPos, Vector2Int dirToIgnore, List<IBubble> bufferList)
        {
            AddToListIfMatch(map, level, startPos, Vector2Int.up, dirToIgnore, bufferList);
            AddToListIfMatch(map, level, startPos, Vector2Int.down, dirToIgnore, bufferList);
            AddToListIfMatch(map, level, startPos, Vector2Int.left, dirToIgnore, bufferList);
            AddToListIfMatch(map, level, startPos, Vector2Int.right, dirToIgnore, bufferList);
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
    }
}