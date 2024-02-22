using System.Collections.ObjectModel;

namespace EVAL.Babalone.Persistence
{
    /// <summary>
    /// Cursed enum
    /// </summary>
    public static class BabaloneBoardSize
    {
        public const int Small = 3;
        public const int Medium = 4;
        public const int Large = 6;

        public static string ToString(int boardSize) => boardSize switch
        {
            Small => nameof(Small),
            Medium => nameof(Medium),
            Large => nameof(Large),
            _ => boardSize.ToString()
        };
    }

    /// <summary>
    /// Represents players of the game.
    /// </summary>
    public enum Player
    {
        A,
        B
    }

    public class BabaloneBoard
    {
        #region Fields

        /// <summary>
        /// Matrix of players on game board.
        /// </summary>
        private readonly Player?[][] _board;

        /// <summary>
        /// Points acheived by each player.
        /// </summary>
        private readonly IDictionary<Player, int> _points;

        #endregion

        #region Properties

        /// <summary>
        /// Player whose turn it is to make a move.
        /// </summary>
        public Player ActivePlayer { get; set; } = Player.A;

        /// <summary>
        /// Width and height of game board.
        /// </summary>
        public int Size => _board.Length;

        /// <summary>
        /// Points achieved by each player.
        /// </summary>
        public IReadOnlyDictionary<Player, int> Points { get; private set; }

        /// <summary>
        /// Returns player occupying cell at
        /// (<paramref name="x"/>, <paramref name="y"/>) coordinates.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Player occupying cell, or <c>null</c> if cell is empty.</returns>
        public Player? this[int x, int y]
        {
            get => _board[x][y];
            set {
                if (_board[x][y] is Player old)
                    --_points[old];
                if (value is Player _new)
                    ++_points[_new];
                _board[x][y] = value;
            }
        }

        /// <summary>
        /// Index of current turn that starts from 1 and
        /// increments each time a player makes a move.
        /// </summary>
        public int Turn { get; set; } = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BabaloneBoard"/> class.
        /// </summary>
        /// <param name="boardSize"></param>
        public BabaloneBoard(int boardSize)
        {
            _board = new Player?[boardSize][];
            // Default 0 points for every player
            _points = Enum.GetValues<Player>().ToDictionary((p => p), (_ => 0));
            for (int i = 0; i < boardSize; i++)
            {
                _board[i] = new Player?[boardSize];
            }
            Points = new ReadOnlyDictionary<Player, int>(_points);
        }

        #endregion
    }
}
