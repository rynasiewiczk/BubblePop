using UnityEngine;

namespace View
{
    public class PieceViewSet
    {
        public Color InnerColor;
        public Color OuterColor;

        public PieceViewSet(Color innerColor, Color outerColor)
        {
            InnerColor = innerColor;
            OuterColor = outerColor;
        }
    }
}