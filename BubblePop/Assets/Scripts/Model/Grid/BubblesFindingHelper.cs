using System;
using System.Collections.Generic;
using Project.Pieces;
using UnityEngine;

namespace Project.Grid
{
    public static class BubblesFindingHelper
    {
        private const int BUBBLE_LEVEL_TO_PROVIDE_TO_IFNORE_LEVEL_CHECK = -1;

        private static readonly Vector2Int[] _leftRowCheckVectors = new Vector2Int[]
        {
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1),

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
            var oppositeDir = GetOppositeDir(dirToCheck, map.GetGridSideForRow(startPos.y));

            var posToCheck = startPos + dirToCheck;

            var bubbleAtPosition = map.GetBubbleAtPositionOrNull(posToCheck);

            if (dirToCheck == dirToIgnore || bubbleAtPosition == null || bufferList.Contains(bubbleAtPosition))
            {
                return;
            }

            if (!BubbleIsMatch(map, bubbleAtPosition, level))
            {
                return;
            }

            bufferList.Add(bubbleAtPosition);
            GetNeighboursWithLevel(map, level, posToCheck, oppositeDir, bufferList);
        }

        private static Vector2Int GetOppositeDir(Vector2Int dir, GridRowSide rowSide)
        {
            if (dir.y == 0)
            {
                return dir.x == 1 ? new Vector2Int(-1, 0) : new Vector2Int(1, 0);
            }

            if (dir.y == -1)
            {
                switch (rowSide)
                {
                    case GridRowSide.Left:
                        return dir.x == -1 ? new Vector2Int(0, 1) : new Vector2Int(-1, 1);
                    case GridRowSide.Right:
                        return dir.x == -1 ? new Vector2Int(1, 1) : new Vector2Int(0, 1);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rowSide), rowSide, null);
                }
            }

            if (dir.y == 1)
            {
                switch (rowSide)
                {
                    case GridRowSide.Left:
                        return dir.x == -1 ? new Vector2Int(0, -1) : new Vector2Int(-1, -1);
                    case GridRowSide.Right:
                        return dir.x == -1 ? new Vector2Int(1, -1) : new Vector2Int(0, -1);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rowSide), rowSide, null);
                }
            }

            Debug.LogError("No condition for changing direction was met. Returning given direction.");
            return dir;
        }

        private static bool BubbleIsMatch(IGridMap gridMap, IBubble bubble, int level)
        {
            var result = bubble != null
                         && (bubble.Level.Value == level || BUBBLE_LEVEL_TO_PROVIDE_TO_IFNORE_LEVEL_CHECK == level)
                         && gridMap.IsBubblePlayable(bubble);
            return result;
        }
        
        public static void GetAllConnectedBubbles(this IGridMap gridMap, IBubble startBubble, List<IBubble> bubblesToStay,
            List<IBubble> bufferListClearedOnEntry)
        {
            bufferListClearedOnEntry.Clear();
            bufferListClearedOnEntry = FindBubblesToCollapse(gridMap, BUBBLE_LEVEL_TO_PROVIDE_TO_IFNORE_LEVEL_CHECK, startBubble.Position.Value,
                bufferListClearedOnEntry);

            bubblesToStay.AddRange(bufferListClearedOnEntry);
        }

        public static List<IBubble> GetAllTopPlayableRowBubblesOnGrid(this IGridMap gridMap, GridSettings gridSettings, List<IBubble> list)
        {
            var topPlayableRow = gridSettings.StartGridSize.y - 1;

            var allBubbles = gridMap.GetAllPlayableBubblesOnGrid();
            foreach (var bubble in allBubbles)
            {
                if (bubble.Position.Value.y == topPlayableRow)
                {
                    list.Add(bubble);
                    if (list.Count == gridSettings.StartGridSize.x)
                    {
                        return list;
                    }
                }
            }

            return list;
        }

        public static List<IBubble> GetBubblesAroundPosition(this IGridMap gridMap, Vector2Int position, List<IBubble> bufferList)
        {
            var rowSide = gridMap.GetGridSideForRow(position.y);
            var checkVectors = rowSide == GridRowSide.Left ? _leftRowCheckVectors : _rightRowCheckVectors;

            foreach (var checkVector in checkVectors)
            {
                var bubble = gridMap.GetBubbleAtPositionOrNull(position + checkVector);
                if (bubble != null)
                {
                    bufferList.Add(bubble);
                }
            }

            return bufferList;
        }

        public static int GetLowestRowWithBubble(List<IBubble> listOfBubbles)
        {
            var lowestRow = int.MaxValue;

            foreach (var playableBubble in listOfBubbles)
            {
                if (playableBubble.Position.Value.y < lowestRow)
                {
                    lowestRow = playableBubble.Position.Value.y;
                }
            }

            return lowestRow;
        }

        public static bool IsBubblePlayable(this IGridMap gridMap, IBubble bubble)
        {
            var disposed = bubble.Destroyed.IsDisposed;
            if (disposed)
            {
                return false;
            }

            var bubbleIsBelowTopOfPlayableGrid = bubble.Position.Value.y < gridMap.Size.Value.y;
            return bubbleIsBelowTopOfPlayableGrid;
        }
    }
}