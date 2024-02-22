using EVAL.Babalone.Persistence;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EVAL.Babalone.ViewModel
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly BabaloneViewModel _mainViewModel;

        public event EventHandler? OpenLoadPage;

        public event EventHandler? OpenSaveGame;

        public ICommand LoadGameCommand => new DelegateCommand(_ => OpenLoadPage?.Invoke(this, EventArgs.Empty));

        public ICommand SaveGameCommand => new DelegateCommand(_ => OpenSaveGame?.Invoke(this, EventArgs.Empty));

        public BoardSizeViewModel BoardSize
        {
            get => new(_mainViewModel.BoardSize);
            set => _mainViewModel.BoardSize = value.Size;
        }

        public ObservableCollection<BoardSizeViewModel> BoardSizes { get; } = new()
        {
            new (BabaloneBoardSize.Small),
            new (BabaloneBoardSize.Medium),
            new (BabaloneBoardSize.Large)
        };

        public SettingsPageViewModel(BabaloneViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}
