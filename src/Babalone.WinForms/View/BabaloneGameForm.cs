using EVAL.Babalone.Model;
using EVAL.Babalone.Persistence;
using static EVAL.Babalone.View.BabaloneViewCommon;

namespace EVAL.Babalone.View
{
    public partial class BabaloneGameForm : Form
    {

        #region Subclasses

        /// <summary>
        /// Represents a cell on the game board, which may contain a <see cref="Player"/>.
        /// </summary>
        private sealed class BabaloneCell : Button
        {
            #region Fields

            /// <summary>
            /// Game form which this cell belongs to.
            /// </summary>
            private readonly BabaloneGameForm _parent;

            /// <summary>
            /// Default background color of cell.
            /// </summary>
            private static readonly Color defaultBackColor = SystemColors.ControlLightLight;
            /// <summary>
            /// Background color of cell when highlighted.
            /// </summary>
            private static readonly Color highlightBackColor = SystemColors.ControlDarkDark;

            /// <summary>
            /// Default color of player text in cell.
            /// </summary>
            private static readonly Color defaultForeColor = SystemColors.ControlText;

            /// <summary>
            /// Color of player text in cell when cell is highlighted.
            /// </summary>
            private static readonly Color highlightForeColor = SystemColors.HighlightText;

            #endregion

            #region Properties

            /// <summary>
            /// Vertical coordinate of cell on game board.
            /// </summary>
            public readonly int X;

            /// <summary>
            /// Horizontal coordinate of cell on game board.
            /// </summary>
            public readonly int Y;

            /// <summary>
            /// Player occupying cell.
            /// </summary>
            private Player? CellValue => _parent._model.GetPosition(X, Y);

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref='BabaloneCell'/> class.
            /// </summary>
            /// <param name="_parent">Parent game form object.</param>
            /// <param name="x">Vertical coordinate of cell within game grid.</param>
            /// <param name="y">Horizontal coordinate of cell within game grid.</param>
            public BabaloneCell(BabaloneGameForm _parent, int x, int y)
            {
                this._parent = _parent;
                X = x;
                Y = y;
                Text = PlayerRepr(CellValue);

                ClientSize = _parent._cellSize;
                Margin = new Padding(0);
                Padding = new Padding(0);

                BackColor = defaultBackColor;
                ForeColor = defaultForeColor;
                FlatStyle = FlatStyle.Flat;

                Dock = DockStyle.Fill;
                TabIndex = x * _parent.BoardSize + y;
                Cursor = Cursors.Hand;
                Enabled = true;

                Click += new EventHandler(OnClick);
                MouseEnter += new EventHandler(OnMouseEnter);
                MouseLeave += new EventHandler(OnMouseLeave);
            }

            #endregion

            #region Event handlers

            /// <summary>
            /// Handles click event on game cells.
            /// </summary>
            private void OnClick(object? sender, EventArgs e)
            {
                // Deselect cell if already selected
                if (_parent._highlightedCell == this)
                {
                    _parent.ExitHighlight();
                    return;
                }
                if (_parent._highlightedCell is null)
                {
                    // Do nothing if this cell's player isn't allowed to make a move
                    if (CellValue != _parent._model.ActivePlayer)
                        return;
                    ColorHighlight();
                    _parent.HighlightCell(this);
                }
                else
                {
                    _parent.SelectCell(this);
                }
            }

            /// <summary>
            /// Handles hover event on game cells.
            /// </summary>
            private void OnMouseEnter(object? sender, EventArgs e)
            {
                if (_parent._highlightedCell is null)
                    return;
                ColorHighlight();
                if (_parent._highlightedCell != this)
                {
                    Text = PlayerRepr(_parent._model.ActivePlayer);
                }
            }

            /// <summary>
            /// Handles exiting hover event on game cells.
            /// </summary>
            private void OnMouseLeave(object? sender, EventArgs e)
            {
                ColorDefault();
                UpdateText();
            }

            #endregion

            #region Methods

            /// <summary>
            /// Update Text attribute to match player occupying this cell.
            /// </summary>
            public void UpdateText()
            {
                Text = PlayerRepr(CellValue);
            }

            /// <summary>
            /// Change background and foreground color to highlight color scheme.
            /// </summary>
            private void ColorHighlight()
            {
                BackColor = highlightBackColor;
                ForeColor = highlightForeColor;
            }

            /// <summary>
            /// Change background and foreground color to default color scheme.
            /// </summary>
            private void ColorDefault()
            {
                BackColor = defaultBackColor;
                ForeColor = defaultForeColor;
            }

            /// <summary>
            /// Enable cell and neighboring cells to which
            /// player is able to move from this one.
            /// </summary>
            public void EnableCellAndNeighbors()
            {
                Enabled = true;
                if (X > 0)
                {
                    _parent._cells[X - 1][Y].Enabled = true;
                }
                if (Y > 0)
                {
                    _parent._cells[X][Y - 1].Enabled = true;
                }
                if (X < _parent.BoardSize - 1)
                {
                    _parent._cells[X + 1][Y].Enabled = true;
                }
                if (Y < _parent.BoardSize - 1)
                {
                    _parent._cells[X][Y + 1].Enabled = true;
                }
            }

            #endregion
        }

        #endregion

        #region Constants

        /// <summary>
        /// Size of <see cref="BabaloneCell"/> cells.
        /// </summary>
        private readonly Size _cellSize = new(40, 40);

        #endregion

        #region Fields

        /// <summary>
        /// Model used to represent and manage current state of the game.
        /// </summary>
        private BabaloneModel _model;

        /// <summary>
        /// Represents the game board on the UI, this contains <see cref="BabaloneCell"/> objects.
        /// </summary>
        private TableLayoutPanel _cellContainer;

        /// <summary>
        /// Stores the cell objects contained within <see cref="_cellContainer"/>, in a more easily accessible way.
        /// </summary>
        private BabaloneCell[][] _cells;

        /// <summary>
        /// Stores the cell the current player intends to make a move from. <c>null</c> if no cell is selected.
        /// </summary>
        private BabaloneCell? _highlightedCell;

        #endregion

        #region Properties

        /// <summary>
        /// Width and height of game board.
        /// </summary>
        private int BoardSize => _model.BoardSize;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BabaloneGameForm"/> class.
        /// </summary>
        public BabaloneGameForm()
        {
            InitializeComponent();

            // To stop compiler from whining
            _model = new(new BabaloneFileDataAccess());
            _cellContainer = new();
            _cells = Array.Empty<BabaloneCell[]>();

            NewGame();
        }

        #endregion

        #region Methods
        // (void only)

        /// <summary>
        /// Starts a brand new game of Babalone.
        /// </summary>
        /// <param name="boardSize">Width and height of new game board.</param>
        /// <param name="requireConfirm">Whether ot not the user should be
        ///     prompted for a confirmation to abandon the current game.</param>
        private void NewGame(int boardSize = BabaloneModel.DefaultBoardSize, bool requireConfirm = false)
        {
            BabaloneModel model = new(new BabaloneFileDataAccess(), boardSize);
            NewGame(model, requireConfirm);
        }

        /// <summary>
        /// Starts a brand new game of Babalone.
        /// </summary>
        /// <param name="model">Model to be used </param>
        /// <param name="requireConfirm">Whether ot not the user should be prompted for a confirmation to abandon the current game.</param>
        private void NewGame(BabaloneModel model, bool requireConfirm = false)
        {
            if (requireConfirm)
            {
                DialogResult d = MessageBox.Show(
                    "Start new game? You will lose your current game!",
                    "New game?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1); // The user probably pressed the button on purpose
                if (d != DialogResult.Yes)
                    return;
            }
            _model = model;
            ClientSize = new(
                (model.BoardSize + 2) * _cellSize.Width,
                (model.BoardSize + 2) * _cellSize.Height
                    + menuStrip.ClientSize.Height
                    + toolStrip.Height
                );
            AddEventHandlers();
            InitializeCellContainer();
            InitializeCells();
            UpdateToolstrip();
        }

        /// <summary>
        /// Assigns appropriate event handlers to model's events.
        /// </summary>
        private void AddEventHandlers()
        {
            _model.CellChanged += new EventHandler<BabaloneCellChangedEventArgs>(UpdateCell);
            _model.GameOver += new EventHandler<BabaloneGameOverEventArgs>(GameOver);
            _model.GameAdvanced += new EventHandler<EventArgs>(UpdateToolstrip);
        }

        /// <summary>
        /// Initializes <see cref="_cellContainer"/> with a size
        /// to match that of the game board.
        /// </summary>
        private void InitializeCellContainer()
        {
            if (_cellContainer is not null)
            {
                Controls.Remove(_cellContainer);
            }
            _cellContainer = new TableLayoutPanel()
            {
                Name = "cellContainer",
                Dock = DockStyle.Fill,
                RowCount = BoardSize + 2,
                ColumnCount = BoardSize + 2,
                GrowStyle = TableLayoutPanelGrowStyle.FixedSize,
            };
            Controls.Add(_cellContainer);
            _cellContainer.BringToFront(); // Otherwise it docks below menu strip
        }

        /// <summary>
        /// Fills <see cref="_cells"/> and <see cref="_cellContainer"/> with board cells.
        /// </summary>
        private void InitializeCells()
        {
            _highlightedCell = null;
            _cells = new BabaloneCell[BoardSize][];

            for (int i = 0; i < BoardSize; ++i)
            {
                _cellContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, _cellSize.Width));
                _cellContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, _cellSize.Height));

                _cells[i] = new BabaloneCell[BoardSize];
                for (int j = 0; j < BoardSize; ++j)
                {
                    BabaloneCell c = new(this, i, j);
                    _cellContainer.Controls.Add(c);
                    _cellContainer.SetCellPosition(c, new TableLayoutPanelCellPosition(j + 1, i + 1));
                    _cells[i][j] = c;
                }
            }
        }

        /// <summary>
        /// Updates toolstrip text to match game status,
        /// including currently active player and turn count.
        /// </summary>
        private void UpdateToolstrip()
        {
            playingToolStripLabel.Text = PlayerRepr(_model.ActivePlayer);
            turnToolStripLabel.Text = $"{_model.Turn}/{_model.MaxTurns}";
        }

        /// <summary>
        /// If a cell is highlighted, push it to target;
        /// otherwise highlight target cell.
        /// </summary>
        /// If target cell is already highlighted, exit highlight.
        /// <param name="target">Cell to push to or highlight.</param>
        private void SelectCell(BabaloneCell target)
        {
            if (_highlightedCell is null)
            {
                HighlightCell(target);
            }
            else if (_highlightedCell == target)
            {
                ExitHighlight();
            }
            else
            {
                _model.Push(_highlightedCell.X, _highlightedCell.Y, target.X, target.Y);
                ExitHighlight();
            }
        }

        /// <summary>
        /// Selects and visually highlights the cell 
        /// the active player intends to move.
        /// </summary>
        /// <param name="target">Cell to select.</param>
        private void HighlightCell(BabaloneCell target)
        {
            if (_model.GetPosition(target.X, target.Y) == null)
                return;

            if (_highlightedCell is not null)
                ExitHighlight();

            _highlightedCell = target;
            foreach (BabaloneCell[] row in _cells)
            {
                foreach (BabaloneCell c in row)
                {
                    c.Enabled = false;
                }
            }
            target.FlatStyle = FlatStyle.Standard;
            target.EnableCellAndNeighbors();
            target.Focus();
        }

        /// <summary>
        /// Deselects and removes visual highlight from
        /// cell the active player had previously marked.
        /// </summary>
        private void ExitHighlight()
        {
            if (_highlightedCell is BabaloneCell b)
                b.FlatStyle = FlatStyle.Flat;
            _highlightedCell = null;
            foreach (BabaloneCell[] row in _cells)
            {
                foreach (BabaloneCell c in row)
                {
                    c.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Saves current game state to a *.baba file.
        /// </summary>
        /// <returns>Whether save has successfully completed.</returns>
        private async Task<bool> SaveGameFile()
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return false;
            try
            {
                await _model.SaveGameAsync(saveFileDialog.FileName);
            }
            catch (BabaloneDataException e)
            {
                MessageBox.Show(
                    "Couldn't save game" +
                    Environment.NewLine +
                    "Error message:" +
                    Environment.NewLine +
                    e.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                return false;
            }
            return true;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Starts a brand new game of Babalone on a 3x3 board.
        /// </summary>
        private void NewGameSmall(object sender, EventArgs e)
        {
            NewGame(BabaloneBoardSize.Small, true);
        }

        /// <summary>
        /// Starts a brand new game of Babalone on a 4x4 board.
        /// </summary>
        private void NewGameMedium(object sender, EventArgs e)
        {
            NewGame(BabaloneBoardSize.Medium, true);
        }

        /// <summary>
        /// Starts a brand new game of Babalone on a 6x6 board.
        /// </summary>
        private void NewGameLarge(object sender, EventArgs e)
        {
            NewGame(BabaloneBoardSize.Large, true);
        }

        /// <summary>
        /// Starts a brand new game of Babalone with 
        /// a same sized board as the current one.
        /// </summary>
        private void RestartGame(object sender, EventArgs e)
        {
            NewGame(BoardSize, true);
        }

        /// <summary>
        /// Updates a cell's text with its new value.
        /// </summary>
        private void UpdateCell(object? sender, BabaloneCellChangedEventArgs e)
        {
            _cells[e.X][e.Y].UpdateText();
        }

        /// <summary>
        /// Updates toolstrip text to match game status,
        /// including currently active player and turn count.
        /// </summary>
        private void UpdateToolstrip(object? sender, EventArgs e)
        {
            UpdateToolstrip();
        }

        /// <summary>
        /// Announces game results and starts a new game.
        /// </summary>
        private void GameOver(object? sender, BabaloneGameOverEventArgs e)
        {
            string pointsStr = string.Join(
                Environment.NewLine,
                e.Points.Select(
                    kvp => $"{PlayerRepr(kvp.Key)}: {kvp.Value}"
                    )
                );
            string winnerMsg = "The game was a tie!";
            if (e.Winner is Player p)
            {
                winnerMsg = $"Player {Enum.GetName(p)} ({PlayerRepr(p)}) won the game!";
            }

            MessageBox.Show(
                winnerMsg +
                Environment.NewLine +
                Environment.NewLine +
                "Points:" +
                Environment.NewLine +
                pointsStr,
                "Game over!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);

            NewGame(BoardSize);
        }

        /// <summary>
        /// Saves current game state to a *.baba file.
        /// </summary>
        private async void SaveGameFile(object? sender, EventArgs _)
        {
            await SaveGameFile();
        }

        /// <summary>
        /// Loads game state from a *.baba file.
        /// </summary>
        private async void LoadGameFile(object? sender, EventArgs _)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                BabaloneModel model = await _model.LoadGameAsync(openFileDialog.FileName);
                NewGame(model, true);
            }
            catch (BabaloneDataException e)
            {
                MessageBox.Show(
                    "Couldn't load game" +
                    Environment.NewLine +
                    "Error message:" +
                    Environment.NewLine +
                    e.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }

        /// <summary>
        /// Saves current game state to a *.baba file and exits the game.
        /// </summary>
        /// <remarks>Doesn't exit the game if saving was cancelled or otherwise unsuccessful.</remarks>
        private async void SaveGameFileAndQuit(object sender, EventArgs e)
        {
            if (await SaveGameFile())
                Close();
        }

        /// <summary>
        /// Exits the game after user confirmation.
        /// </summary>
        private void CloseAfterConfirmation(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show(
                "Are you sure you want to quit without saving the game?",
                "Quit?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1); // The user probably pressed the button on purpose
            if (d == DialogResult.Yes)
                Close();
        }

        #endregion
    }
}
