using Enums;
using Project.Pieces;
using UnityEngine;

namespace Project.Aiming
{
    public class PieceAimedData
    {
        public readonly IBubble Piece;
        public readonly PieceSide AimedSide;
        public readonly Vector2[] PathFromAimingPosition;

        public PieceAimedData(IBubble piece, PieceSide aimedSide, Vector2[] pathFromAimingPosition)
        {
            Piece = piece;
            AimedSide = aimedSide;
            PathFromAimingPosition = pathFromAimingPosition;
        }
    }
}