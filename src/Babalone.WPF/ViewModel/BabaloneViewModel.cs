using EVAL.Babalone.Model;
using EVAL.Babalone.Persistence;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

        private static readonly IBabaloneDataAccess _dataAccess = new BabaloneFileDataAccess();

        #endregion

        #region Commands

        public ICommand NewGameCommand { get; private set; }

        public ICommand SaveGameCommand { get; private set; }

        public ICommand LoadGameCommand { get; private set; }

        #endregion

        #region Properties

        public string ActivePlayer => PlayerRepr(_model.ActivePlayer);

        public int BoardSize => _model.BoardSize;

        public int Turn => _model.Turn;

        public int MaxTurns => _model.MaxTurns;

        public bool IsBoardSizeSmall
        {
            get => BoardSize == BabaloneBoardSize.Small;
            set => SetBoardSize(BabaloneBoardSize.Small);
        }

        public bool IsBoardSizeMedium
        {
            get => BoardSize == BabaloneBoardSize.Medium;
            set => SetBoardSize(BabaloneBoardSize.Medium);
        }

        public bool IsBoardSizeLarge
        {
            get => BoardSize == BabaloneBoardSize.Large;
            set => SetBoardSize(BabaloneBoardSize.Large);
        }

        public ObservableCollection<BabaloneCell> Cells { get; private set; }

        #endregion

        #region Constructors

        public BabaloneViewModel()
        {
            _model = new BabaloneModel(new BabaloneFileDataAccess());  // To stop compiler from whining
            Cells = new ObservableCollection<BabaloneCell>();
            NewGame();  // Trigger property change events

            NewGameCommand = new DelegateCommand(_ => NewGame());
            SaveGameCommand = new DelegateCommand(async _ => await SaveGameFileAsync());
            LoadGameCommand = new DelegateCommand(async _ => await LoadGameFileAsync());
        }

        #endregion

        #region Methods

        private void SetBoardSize(int boardSize)
        {
            if (BoardSize == boardSize)
                return;
            NewGame(boardSize);
        }

        public void NewGame() => NewGame(BoardSize);

        private void NewGame(int boardSize) => NewGame(new BabaloneModel(_dataAccess, boardSize));

        private void NewGame(BabaloneModel model)
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
            _model.GameOver += GameOver;

            OnPropertyChanged(nameof(Turn));
            OnPropertyChanged(nameof(MaxTurns));
            OnPropertyChanged(nameof(BoardSize));
            OnPropertyChanged(nameof(ActivePlayer));
            OnPropertyChanged(nameof(IsBoardSizeSmall));
            OnPropertyChanged(nameof(IsBoardSizeMedium));
            OnPropertyChanged(nameof(IsBoardSizeLarge));
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

        public async Task<bool> SaveGameFileAsync()
        {
            SaveFileDialog dialog = new()
            {
                Title = "Save game",
                DefaultExt = "baba",
                FileName = "BabaloneGame",
                Filter = "Babalone game (*.baba)|*.baba|All files|*.*"
            };
            if (dialog.ShowDialog() != true)
                return false;
            try
            {
                await _model.SaveGameAsync(dialog.FileName);
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
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
            return true;
        }

        public async Task LoadGameFileAsync()
        {
            OpenFileDialog dialog = new()
            {
                Title = "Load game",
                DefaultExt = "baba",
                FileName = "BabaloneGame",
                Filter = "Babalone game (*.baba)|*.baba|All files|*.*"
            };
            if (dialog.ShowDialog() != true)
                return;
            try
            {
                BabaloneModel model = await _model.LoadGameAsync(dialog.FileName);
                NewGame(model);
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
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
        }

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
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.OK);

            NewGame();
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
