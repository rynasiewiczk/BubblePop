using System;
using System.Collections.Generic;
using Project.Pieces;
using UnityEngine;

namespace Project.Grid
{
    public static class PiecesFindingHelper
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

        public static List<IPiece> FindBubblesToCollapse(this IGridMap map, IPiece startPiece, List<IPiece> bufferList)
        {
            bufferList.Add(startPiece);
            var bubbleLevel = startPiece.Level.Value;

            GetNeighboursWithLevel(map, bubbleLevel, startPiece.Position.Value, Vector2Int.zero, bufferList);

            return bufferList;
        }

        public static List<IPiece> FindBubblesToCollapse(this IGridMap gridMap, int level, Vector2Int position, List<IPiece> bufferList)
        {
            var bubbleAtPosition = gridMap.GetPieceAtPositionOrNull(position);
            if (bubbleAtPosition != null)
            {
                bufferList.Add(bubbleAtPosition);
            }

            GetNeighboursWithLevel(gridMap, level, position, Vector2Int.zero, bufferList);
            return bufferList;
        }

        private static void GetNeighboursWithLevel(IGridMap map, int level, Vector2Int startPos, Vector2Int dirToIgnore, List<IPiece> bufferList)
        {
            var rowSide = map.GetGridSideForRow(startPos.y);
            var checkVectors = rowSide == GridRowSide.Left ? _leftRowCheckVectors : _rightRowCheckVectors;

            foreach (var checkVector in checkVectors)
            {
                AddToListIfMatch(map, level, startPos, checkVector, dirToIgnore, bufferList);
            }
        }

        private static void AddToListIfMatch(IGridMap map, int level, Vector2Int startPos, Vector2Int dirToCheck, Vector2Int dirToIgnore,
            List<IPiece> bufferList)
        {
            var oppositeDir = GetOppositeDir(dirToCheck, map.GetGridSideForRow(startPos.y));

            var posToCheck = startPos + dirToCheck;

            var bubbleAtPosition = map.GetPieceAtPositionOrNull(posToCheck);

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

        private static bool BubbleIsMatch(IGridMap gridMap, IPiece piece, int level)
        {
            var result = piece != null
                         && (piece.Level.Value == level || BUBBLE_LEVEL_TO_PROVIDE_TO_IFNORE_LEVEL_CHECK == level)
                         && gridMap.IsBubblePlayable(piece);
            return result;
        }
        
        public static void GetAllConnectedBubbles(this IGridMap gridMap, IPiece startPiece, List<IPiece> bubblesToStay,
            List<IPiece> bufferListClearedOnEntry)
        {
            bufferListClearedOnEntry.Clear();
            bufferListClearedOnEntry = FindBubblesToCollapse(gridMap, BUBBLE_LEVEL_TO_PROVIDE_TO_IFNORE_LEVEL_CHECK, startPiece.Position.Value,
                bufferListClearedOnEntry);

            bubblesToStay.AddRange(bufferListClearedOnEntry);
        }

        public static List<IPiece> GetAllTopPlayableRowBubblesOnGrid(this IGridMap gridMap, GridSettings gridSettings, List<IPiece> list)
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

        public static List<IPiece> GetBubblesAroundPosition(this IGridMap gridMap, Vector2Int position, List<IPiece> bufferList)
        {
            var rowSide = gridMap.GetGridSideForRow(position.y);
            var checkVectors = rowSide == GridRowSide.Left ? _leftRowCheckVectors : _rightRowCheckVectors;

            foreach (var checkVector in checkVectors)
            {
                var bubble = gridMap.GetPieceAtPositionOrNull(position + checkVector);
                if (bubble != null)
                {
                    bufferList.Add(bubble);
                }
            }

            return bufferList;
        }

        public static int GetLowestRowWithBubble(List<IPiece> listOfBubbles)
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

        public static bool IsBubblePlayable(this IGridMap gridMap, IPiece piece)
        {
            var disposed = piece.Destroyed.IsDisposed;
            if (disposed)
            {
                return false;
            }

            var bubbleIsBelowTopOfPlayableGrid = piece.Position.Value.y < gridMap.Size.Value.y;
            return bubbleIsBelowTopOfPlayableGrid;
        }
    }
}