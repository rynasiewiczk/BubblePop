namespace Enums
{
    public enum GamePlayState
    {
        None = 0,

        Idle,
        Aiming,
        PieceFlying,
        PlacingPieceOnGrid,
        PiecesCombining,
        WaitingForPiecesCombine,
        DropPiecesAfterCombining,
        ScrollRows,
        FillPiecesAboveTop,
    }
}