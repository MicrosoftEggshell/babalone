using EVAL.Babalone.Persistence;

namespace EVAL.Babalone.View
{
    public static class BabaloneViewCommon
    {
        #region Functions
        // (non-void only)

        /// <summary>
        /// Returns a <see cref="string"/> representation of a
        /// <see cref="Player"/> that can be drawn on the game board.
        /// </summary>
        /// <param name="player">Player to represent.</param>
        /// <returns>Player as a printable value.</returns>
        public static string PlayerRepr(Player? player) => player switch
        {
            Player.A => "⚪",
            Player.B => "⚫",
            _ => "",
        };

        #endregion
    }
}
