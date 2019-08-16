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
        DropAndExplodePiecesAfterCombining,
        ScrollRows,
        FillPiecesAboveTop,
    }
}