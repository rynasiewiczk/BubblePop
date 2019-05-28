using System;
using System.Collections.Generic;
using Enums;
using Project.Pieces;
using UnityEngine;

namespace Project.Grid
{
    public static class GridMapHelper
    {
        private const float DISTANCE_BETWEEN_BUBBLES = 1f;
        private const float HALF_OF_DISTANCE_BETWEEN_BUBBLES = DISTANCE_BETWEEN_BUBBLES / 2;

        public static Vector2Int GetPositionToSpawnPiece(this IGridMap gridMap, IPiece piece, PieceSide aimedSide, Vector2 hitPointRelativeToPiecesCenter)
        {
            const int EDGE_ANGLE = 45;

            var angle = GetAngleOfPieceHit(hitPointRelativeToPiecesCenter);

            var position = piece.Position.Value;
            Vector2Int direction;

            var rowSideOfHit = gridMap.GridRowSidesMap[position.y];

            switch (aimedSide)
            {
                case PieceSide.TopLeft:
                    direction = new Vector2Int(-1, 0);
                    break;
                case PieceSide.TopRight:
                    direction = new Vector2Int(1, 0);
                    break;
                case PieceSide.BottomLeft:
                    direction = rowSideOfHit == GridRowSide.Left ? new Vector2Int(-1, -1) : new Vector2Int(0, -1);
                    if (BubbleExistsAtPosition(gridMap, piece.Position.Value + direction))
                    {
                        if (rowSideOfHit == GridRowSide.Left)
                        {
                            direction = angle <= EDGE_ANGLE ? new Vector2Int(-1, 0) : new Vector2Int(0, -1);
                        }
                        else
                        {
                            direction = angle <= EDGE_ANGLE ? new Vector2Int(-1, 0) : new Vector2Int(1, -1);
                        }
                    }

                    break;
                case PieceSide.BottomRight:
                    direction = rowSideOfHit == GridRowSide.Left ? new Vector2Int(0, -1) : new Vector2Int(1, -1);
                    if (BubbleExistsAtPosition(gridMap, piece.Position.Value + direction))
                    {
                        if (rowSideOfHit == GridRowSide.Left)
                        {
                            direction = angle <= EDGE_ANGLE ? new Vector2Int(1, 0) : new Vector2Int(-1, -1);
                        }
                        else
                        {
                            direction = angle <= EDGE_ANGLE ? new Vector2Int(1, 0) : new Vector2Int(0, -1);
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var result = position + direction;
            return result;
        }

        private static float GetAngleOfPieceHit(Vector2 hitPointRelativeToPiecesCenter)
        {
            const float RADUIS = .5f;

            var sinus = Mathf.Abs(hitPointRelativeToPiecesCenter.y) / RADUIS;
            var radians = Mathf.Asin(sinus);
            var angles = Mathf.Rad2Deg * radians;
            return angles;
        }


        public static Vector2Int GetCellPositionByWorldPosition(this IGridMap gridMap, Vector2 position)
        {
            var column = Mathf.RoundToInt(position.x);
            var row = GetRowByWorldHeight(position.y);
            return new Vector2Int(column, row);
        }

        public static Vector2 GetViewPosition(this IGridMap gridMap, Vector2 position)
        {
            var intPosition = new Vector2Int((int) position.x, (int) position.y);
            return gridMap.GetViewPosition(intPosition);
        }

        public static Vector2 GetViewPosition(this IGridMap gridMap, Vector2Int position)
        {
            var offsetX = 0f;
            if (gridMap.GridRowSidesMap[position.y] == GridRowSide.Right)
            {
                offsetX += .5f;
            }

            var viewPositionInX = position.x + offsetX;
            var viewPositionInY = GetHeightOfRowsInWorldPosition(position.y);
            return new Vector2(viewPositionInX, viewPositionInY);
        }

        public static List<IPiece> GetAllPlayableBubblesOnGrid(this IGridMap gridMap)
        {
            var list = new List<IPiece>();
            foreach (var bubble in gridMap.PiecesRegistry)
            {
                if (gridMap.IsPiecePlayable(bubble))
                {
                    list.Add(bubble);
                }
            }

            return list;
        }

        public static List<ICell> GetAllEmptyCellsAboveTheGrid(this IGridMap gridMap, int gridTopLine)
        {
            var list = new List<ICell>();
            foreach (var cell in gridMap.CellsRegistry)
            {
                if (cell != null && cell.Position.y >= gridTopLine)
                {
                    var bubbleAtPosition = gridMap.GetPieceAtPositionOrNull(cell.Position);
                    if (bubbleAtPosition == null)
                    {
                        list.Add(cell);
                    }
                }
            }

            return list;
        }

        public static List<IPiece> GetAllBubblesOnGrid(this IGridMap gridMap)
        {
            var list = new List<IPiece>();
            foreach (var bubble in gridMap.PiecesRegistry)
            {
                if (bubble != null)
                {
                    list.Add(bubble);
                }
            }

            return list;
        }

        public static float GetHeightOfRowsInWorldPosition(int rows)
        {
            var heightBetweenRows = Mathf.Pow(Mathf.Pow(DISTANCE_BETWEEN_BUBBLES, 2) - Mathf.Pow(HALF_OF_DISTANCE_BETWEEN_BUBBLES, 2), .5f);

            var result = heightBetweenRows * rows;
            return result;
        }

        private static int GetRowByWorldHeight(float height)
        {
            var heightBetweenRows = Mathf.Pow(Mathf.Pow(DISTANCE_BETWEEN_BUBBLES, 2) - Mathf.Pow(HALF_OF_DISTANCE_BETWEEN_BUBBLES, 2), .5f);

            var row = 0;

            while (height > 0.1f)
            {
                row++;
                height -= heightBetweenRows;
            }

            return row;
        }

        private static bool BubbleExistsAtPosition(this IGridMap gridMap, Vector2Int position)
        {
            var bubble = gridMap.GetPieceAtPositionOrNull(position);
            return bubble != null;
        }
    }
}