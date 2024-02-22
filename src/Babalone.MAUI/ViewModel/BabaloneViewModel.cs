using EVAL.Babalone.Model;
using EVAL.Babalone.Persistence;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static EVAL.Babalone.View.BabaloneViewCommon;

namespace EVAL.Babalone.ViewModel
{
    public class BabaloneViewModel : ViewModelBase
    {

        #region Fields

        /// <summary>
        /// Model used to represent and manage current state of the game.
        /// </summary>
        private BabaloneModel _model;

        private BabaloneCell? _selectedCell;

        #endregion

        #region Commands

        public ICommand NewGameCommand { get; private set; }

        public ICommand SettingsCommand { get; private set; }

        #endregion

        #region Properties

        public string ActivePlayer => PlayerRepr(_model.ActivePlayer);

        public int BoardSize
        {
            get => _model.BoardSize;
            set
            {
                if (BoardSize == value)
                    return;
                StartNewGame(value);  // Handles OnPropertyChanged
            }
        }
        public int Turn => _model.Turn;

        public int MaxTurns => _model.MaxTurns;

        public bool IsGameOver => _model.IsGameOver;

        public ObservableCollection<BabaloneCell> Cells { get; } = new();

        public RowDefinitionCollection GameBoardRows
            => new(Enumerable.Repeat(new RowDefinition(GridLength.Star), BoardSize).ToArray());

        public ColumnDefinitionCollection GameBoardColumns
            => new(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), BoardSize).ToArray());

        #endregion

        #region Events

        public event EventHandler<NewGameEventArgs>? NewGame;

        public event EventHandler? OpenSettings;

        #endregion

        #region Constructors

        public BabaloneViewModel(BabaloneModel model)
        {
            _model = model;
            LoadModel(model);

            NewGameCommand = new DelegateCommand(_ => StartNewGame());
            SettingsCommand = new DelegateCommand(_ => OpenSettings?.Invoke(this, EventArgs.Empty));
        }

        #endregion

        #region Methods

        public void StartNewGame() => StartNewGame(BoardSize);

        public void StartNewGame(int boardSize) => NewGame?.Invoke(this, new(boardSize));

        protected internal void LoadModel(BabaloneModel model)
        {
            _model = model;
            Cells.Clear();
            for (int i = 0; i < BoardSize; ++i)
            {
                for (int j = 0; j < BoardSize; ++j)
                {
                    // Could probabaly be Cells.Add
                    Cells.Insert(BoardSize * i + j, new BabaloneCell(this, i, j)
                    {
                        OnClick = new DelegateCommand(OnCellClick)
                    });
                }
            }

            _model.CellChanged += CellChanged;
            _model.GameAdvanced += GameAdvanced;

            OnPropertyChanged(nameof(Turn));
            OnPropertyChanged(nameof(MaxTurns));
            OnPropertyChanged(nameof(BoardSize));
            OnPropertyChanged(nameof(ActivePlayer));
            OnPropertyChanged(nameof(GameBoardRows));
            OnPropertyChanged(nameof(GameBoardColumns));
        }

        private void SelectCell(BabaloneCell cell, out bool needUpdate)
        {
            needUpdate = true;
            if (_selectedCell is null)
            {
                if (cell.Text != ActivePlayer)
                {
                    // Invalid selection
                    needUpdate = false;
                }
                else
                {
                    // Select cell
                    _selectedCell = cell;
                }
                return;
            }
            if (_selectedCell == cell)
            {
                // Deselect cell
                _selectedCell = null;
                return;
            }
            // Push cell
            _model.Push(_selectedCell.X, _selectedCell.Y, cell.X, cell.Y);
            _selectedCell = null;
        }

        public async Task SaveGameAsync(string path) => await _model.SaveGameAsync(path);

        public async Task LoadGameAsync(string path) => LoadModel(await _model.LoadGameAsync(path));

        #endregion

        #region Functions

        protected internal string GetPosition(int x, int y) => PlayerRepr(_model.GetPosition(x, y));

        protected internal bool IsSelectedOrNeighbor(BabaloneCell cell) =>
            _selectedCell is not BabaloneCell selected ||
            selected == cell ||
            (
                (selected.X - cell.X) * (selected.Y - cell.Y) == 0 &&
                (
                    (selected.Y - cell.Y) is 1 or -1 ||
                    (selected.X - cell.X) is 1 or -1
                )
            );

        #endregion

        #region Game event handlers

        public void CellChanged(object? sender, BabaloneCellChangedEventArgs e)
        {
            Cells[e.X * BoardSize + e.Y].UpdateText();
        }

        public void GameAdvanced(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ActivePlayer));
            OnPropertyChanged(nameof(Turn));
        }

        #endregion

        #region View event handlers

        public void OnCellClick(object? p)
        {
            if (p is not BabaloneCell cell)
            {
                return;
            }
            SelectCell(cell, out bool needUpdate);
            if (!needUpdate)
                return;
            foreach (BabaloneCell c in Cells)
                c.OnPropertyChanged(nameof(BabaloneCell.IsActive));
        }

        #endregion
    }
}
