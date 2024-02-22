using EVAL.Babalone.Model;
using System.Collections.ObjectModel;
using static EVAL.Babalone.Persistence.BabaloneMauiCommon;

namespace EVAL.Babalone.ViewModel;

public class StoredGameBrowserViewModel : ViewModelBase
{
    private readonly StoredGameBrowserModel _model;

    public event EventHandler<StoredGameEventArgs>? SaveGameEvent;

    public event EventHandler<StoredGameEventArgs>? LoadGameEvent;

    public DelegateCommand NewSaveCommand { get; private set; }

    public ObservableCollection<StoredGameViewModel> StoredGames { get; } = new();

    public StoredGameBrowserViewModel(StoredGameBrowserModel model)
    {
        _model = model;
        _model.StoreChanged += (_, _) => UpdateStoredGames();

        NewSaveCommand = new DelegateCommand(param =>
        {
            if (string.IsNullOrEmpty(param?.ToString()?.Trim()))
                return;
            SaveGame(Pathify(param) + ".baba");
        });
        UpdateStoredGames();
    }

    internal void UpdateStoredGames()
    {
        StoredGames.Clear();

        foreach (StoredGameModel item in _model.StoredGames)
        {
            StoredGames.Add(new StoredGameViewModel(item)
            {
                LoadGameCommand = new DelegateCommand(LoadGame),
                SaveGameCommand = new DelegateCommand(SaveGame)
            });
        }
    }

    private void LoadGame(object? fileName)
    {
        LoadGameEvent?.Invoke(this, new(Pathify(fileName)));
    }

    private void SaveGame(object? fileName)
    {
        SaveGameEvent?.Invoke(this, new(Pathify(fileName)));
    }
}
