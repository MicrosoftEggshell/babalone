using EVAL.Babalone.Persistence;
using static Babalone.Model.BabaloneCommon;

namespace EVAL.Babalone.Model
{
    /// <summary>
    /// Manages and represents the state of a Babalone game.
    /// </summary>
    public class BabaloneModel
    {
        #region Constants

        /// <summary>
        /// Default width and height of the game board.
        /// </summary>
        public const int DefaultBoardSize = BabaloneBoardSize.Medium;

        #endregion

        #region Fields

        /// <summary>
        /// Game board.
        /// </summary>
        private readonly BabaloneBoard _board;

        /// <summary>
        /// Provides data access for saving and loading the game state.
        /// </summary>
        private readonly IBabaloneDataAccess _dataAccess;

        #endregion

        #region Properties

        /// <summary>
        /// Points achieved by each player.
        /// </summary>
        private IReadOnlyDictionary<Player, int> Points => _board.Points;

        /// <summary>
        /// Index of current turn that starts from 1 and
        /// increments each time a player makes a move.
        /// </summary>
        public int Turn {
            get => _board.Turn;
            private set
            {
                if (IsGameOver)
                    return;
                _board.Turn = value;
                if (_board.Turn >= MaxTurns)
                {
                    IsGameOver = true;
                    EndGame();
                }
            }
        }

        /// <summary>
        /// Player whose turn it is to make a move.
        /// </summary>
        public Player ActivePlayer
        {
            get => _board.ActivePlayer;
            private set => _board.ActivePlayer = value;
        }

        /// <summary>
        /// Width and height of game board.
        /// </summary>
        public int BoardSize => _board.Size;

        /// <summary>
        /// Turn count after which the game terminates.
        /// </summary>
        /// <remarks>5 times the size of the game board (<see cref="BoardSize"/>).</remarks>
        public int MaxTurns => 5 * BoardSize;

        /// <summary>
        /// Whether or not game has reached a conclusion.
        /// </summary>
        public bool IsGameOver { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// This event occurs whenever a cell's value change on the game board.
        /// </summary>
        /// <remarks>May occur on a cell even though its value didn't change.</remarks>
        public event EventHandler<BabaloneCellChangedEventArgs>? CellChanged;

        /// <summary>
        /// This event occurs whenever a player has made their move.
        /// </summary>
        public event EventHandler<EventArgs>? GameAdvanced;

        /// <summary>
        /// This event occurs whenever a game comes to a conculsion.
        /// </summary>
        /// <remarks>Doesn't occur if game is aborted (eg. restarted).</remarks>
        public event EventHandler<BabaloneGameOverEventArgs>? GameOver;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BabaloneModel"/> class.
        /// </summary>
        /// <param name="boardSize">Width and size of game board.</param>
        public BabaloneModel(IBabaloneDataAccess dataAccess, int boardSize = DefaultBoardSize) : this(dataAccess, new BabaloneBoard(boardSize))
        {
            PlaceStarters();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BabaloneModel"/> class.
        /// </summary>
        /// <param name="board">Board on which game is played.</param>
        private BabaloneModel(IBabaloneDataAccess dataAccess, BabaloneBoard board)
        {
            _dataAccess = dataAccess;
            _board = board;

            bool anyNoPoints = false;
            bool anyHavePoints = false;
            foreach (int kvp in board.Points.Values)
            {
                anyHavePoints |= kvp > 0;
                anyNoPoints |= kvp == 0;
            }

            if ((anyHavePoints && anyNoPoints) ||
                Turn >= MaxTurns)
                EndGame();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates a cell's value.
        /// </summary>
        /// <param name="x">X coordinate of cell to be updated.</param>
        /// <param name="y">Y coordinate of cell to be updated.</param>
        /// <param name="newVal">New player occupying cell (or <c>null</c> for empty cell)</param>
        private void Place(int x, int y, Player? newVal)
        {
            if (IsGameOver)
                return;

            _board[x, y] = newVal;
            CellChanged?.Invoke(this, new(x, y));
        }

        /// <summary>
        /// Place <paramref name="starterCount"/> player tokens for each player.
        /// </summary>
        /// <param name="starterCount">Number of tokens to place per player.</param>
        private void PlaceStarters(int starterCount)
        {
            if (2 * starterCount > BoardSize * BoardSize)
                throw new ArgumentException($"Can't place more starters (2 * {starterCount}) than there are slots on the board!");
            if (IsGameOver)
                return;
                

            // Generate shuffled array of 1D coordinates
            int[] allCoordsShuffled = ShuffledRange(BoardSize * BoardSize);

            foreach (int i in allCoordsShuffled.Take(2 * starterCount))
            {
                // Convert 1D coordinate to 2D coordinate pair
                int x = Math.DivRem(i, BoardSize, out int y);

                Place(x, y, ActivePlayer);
                // Alternate players - ensure equal number of starters
                ActivePlayer = NextPlayer();
            }
        }

        /// <summary>
        /// Place default number of player tokens for each player.
        /// </summary>
        /// <remarks>Current default is set to game board's
        /// width/height (<see cref="BoardSize"/>).</remarks>
        private void PlaceStarters()
        {
            PlaceStarters(BoardSize);
        }

        /// <summary>
        /// Pushes player token on board and transfers control to next player.
        /// </summary>
        /// <param name="fromX">X coordinate of token to be pushed.</param>
        /// <param name="fromY">Y coordinate of token to be pushed.</param>
        /// <param name="toX">X coordinate of target cell.</param>
        /// <param name="toY">Y coordinate of target cell.</param>
        /// <exception cref="ArgumentException">If move is illegal.</exception>
        public void Push(int fromX, int fromY, int toX, int toY)
        {
            // Let exception occur if move is invalid
            ValidatePush(fromX, fromY, toX, toY);
            PushUnsafe(fromX, fromY, toX, toY);
            AdvanceTurn();
        }

        /// <summary>
        /// Throws <see cref="ArgumentException"/> if push from
        /// (<paramref name="fromX"/>, <paramref name="fromY"/>) to
        /// (<paramref name="toX"/>, <paramref name="toY"/>) is invalid.
        /// </summary>
        /// <param name="fromX">X coordinate of token to be pushed.</param>
        /// <param name="fromY">Y coordinate of token to be pushed.</param>
        /// <param name="toX">X coordinate of target cell.</param>
        /// <param name="toY">Y coordinate of target cell.</param>
        /// <exception cref="ArgumentException">If move is illegal.</exception>
        private void ValidatePush(int fromX, int fromY, int toX, int toY)
        {
            List<string> issues = new();
            string boardSize = $"{nameof(BoardSize)} ({BoardSize})";

            if (fromX < 0)
                issues.Add($"Source X coordinate ({fromX}) is negative");
            else if (fromX >= BoardSize)
                issues.Add($"Source X coordinate ({fromX}) is larger than {boardSize}");

            if (fromY < 0)
                issues.Add($"Source Y coordinate ({fromY}) is negative");
            else if (fromY >= BoardSize)
                issues.Add($"Source Y coordinate ({fromY}) is larger than {boardSize}");

            if (toX < 0)
                issues.Add($"Destination X coordinate ({toX}) is negative");
            else if (toX >= BoardSize)
                issues.Add($"Destination X coordinate ({toX}) is larger than {boardSize}");

            if (toY < 0)
                issues.Add($"Destination Y coordinate ({toY}) is negative");
            else if (toY >= BoardSize)
                issues.Add($"Destination Y coordinate ({toY}) is larger than {boardSize}");

            int dx = toX - fromX;
            int dy = toY - fromY;

            if (dx is > 1 or < -1)
                issues.Add($"Source and target are too far from each other on the X axis ({dx})");
            if (dy is > 1 or < -1)
                issues.Add($"Source and target are too far from each other on the Y axis ({dy})");
            if (dx * dy != 0)
                issues.Add($"Can't move on X and Y axes at the same time (dx = {dx}, dy = {dy})");

            if (_board[fromX, fromY] is null)
                issues.Add($"No token to push at ({fromX}, {fromY})");

            if (_board[fromX, fromY] != ActivePlayer)
                issues.Add($"Only the active player can push their token! (({fromX}, {fromY}) = {_board[fromX, fromY]})");

            if (issues.Count > 0)
            {
                throw new ArgumentException(string.Join(Environment.NewLine, issues));
            }
        }

        /// <summary>
        /// Pushes player token on board and transfers control to next player
        /// without checking move for validity.
        /// </summary>
        /// <param name="fromX">X coordinate of token to be pushed.</param>
        /// <param name="fromY">Y coordinate of token to be pushed.</param>
        /// <param name="toX">X coordinate of target cell.</param>
        /// <param name="toY">Y coordinate of target cell.</param>
        private void PushUnsafe(int fromX, int fromY, int toX, int toY)
        {
            int dx = toX - fromX;
            int dy = toY - fromY;
            int curX = fromX;
            int curY = fromY;

            Player? prevCell = null; // Empties source cell on first iteration
            do
            {
                try
                {
                    Player? curCell = _board[curX, curY];
                    Place(curX, curY, prevCell);
                    prevCell = curCell;
                    curX += dx;
                    curY += dy;
                }
                catch (IndexOutOfRangeException)
                {
                    // We've reached the edge of the board
                    if (prevCell is Player p)
                    {
                        // Player p gets pushed off the board
                        if (Points[p] <= 0)
                        {
                            EndGame();
                        }
                    }
                    break;
                }
            } while (prevCell is not null);
        }

        /// <summary>
        /// Initiates next turn.
        /// </summary>
        private void AdvanceTurn()
        {
            if (IsGameOver)
                return;

            Player nextPlayer = NextPlayer();
            if (nextPlayer == Player.A)
                ++Turn;
            ActivePlayer = nextPlayer;
        
            if (!IsGameOver)
                GameAdvanced?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Player whose turn it is next.
        /// </summary>
        /// <returns>Upcoming player.</returns>
        private Player NextPlayer() => ActivePlayer switch
        {
            Player.A => Player.B,
            Player.B => Player.A,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Terminates game.
        /// </summary>
        private void EndGame()
        {
            IsGameOver = true;
            GameOver?.Invoke(this, new(Points));
        }

        /// <summary>
        /// Saves state of game to a location at <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Target path.</param>
        public async Task SaveGameAsync(string path)
        {
            await _dataAccess.SaveAsync(path, _board);
        }

        /// <summary>
        /// Loads state of game from location at <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Save path.</param>
        /// <returns>A <see cref="BabaloneModel"/> instance with the loaded game state.</returns>
        public async Task<BabaloneModel> LoadGameAsync(string path)
        {
            return new BabaloneModel(_dataAccess, await _dataAccess.LoadAsync(path));
        }

        #endregion

        #region Getters

        /// <summary>
        /// Getter for player at (<paramref name="x"/>, <paramref name="y"/>)
        /// coordinates on the board.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Player occupying cell, or <c>null</c> if cell is empty.</returns>
        public Player? GetPosition(int x, int y)
        {
            return _board[x, y];
        }

        #endregion
    }
}
