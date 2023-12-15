using EVAL.Babalone.Persistence;

namespace EVAL.Babalone.Model
{
    /// <summary>
    /// Provides data for the <see cref="BabaloneModel.CellChanged"/> event handler.
    /// </summary>
    public class BabaloneCellChangedEventArgs : EventArgs
    {
        /// <summary>
        /// X coordinate of changed cell.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Y coordinate of changed cell
        /// </summary>
        public readonly int Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="BabaloneCellChangedEventArgs"/> class.
        /// </summary>
        /// <param name="x">X coordinate of changed cell.</param>
        /// <param name="y">Y coordinate of changed cell.</param>
        /// <param name="newValue">New value of changed cell.</param>
        public BabaloneCellChangedEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
