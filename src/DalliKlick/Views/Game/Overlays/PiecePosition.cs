using System;

namespace DalliKlick.Views.Game.Overlays
{
    [Flags]
    public enum PiecePosition
    {
        None = 0,
        Top = 1 << 0,
        Right = 1 << 1,
        Bottom = 1 << 2,
        Left = 1 << 3
    }
}