using EVAL.Babalone.View;
using EVAL.Babalone.Model;
using EVAL.Babalone.Persistence;
using EVAL.Babalone.ViewModel;

namespace EVAL.Babalone;
public partial class AppShell : Shell
{
    #region Fields

    private readonly IBabaloneDataAccess _babaloneDataAccess;
    private readonly BabaloneViewModel _babaloneViewModel;
    private BabaloneModel _babaloneGameModel;
    private readonly SettingsPageViewModel _settingsPageViewModel;

    private readonly IStore _store;
    private readonly StoredGameBrowserModel _storedGameBrowserModel;
    private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

    #endregion

    public BabaloneViewModel ViewModel => _babaloneViewModel;

    public AppShell(IBabaloneDataAccess dataAccess)
    {
        InitializeComponent();

        _babaloneDataAccess = dataAccess;
        _babaloneGameModel = new BabaloneModel(_babaloneDataAccess);
        _babaloneViewModel = new BabaloneViewModel(_babaloneGameModel);
        _settingsPageViewModel = new SettingsPageViewModel(_babaloneViewModel);

        // ez több absztrakció mint a picasso összes de hogy mi a fasznak azt senki se tudja
        _store = new BabaloneStore();
        _storedGameBrowserModel = new StoredGameBrowserModel(_store);
        _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);

        _babaloneViewModel.OpenSettings += SettingsEventHandler;
        _babaloneViewModel.NewGame += NewGame;
        _settingsPageViewModel.OpenLoadPage += OpenLoadPageEventHandler;
        _settingsPageViewModel.OpenSaveGame += OpenSavePageEventHandler;
        _storedGameBrowserViewModel.SaveGameEvent += SaveGameAsync;
        _storedGameBrowserViewModel.LoadGameEvent += LoadGameAsync;

        BindingContext = _babaloneViewModel;
    }

    private async void SettingsEventHandler(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage(_settingsPageViewModel));
    }

    private async void OpenLoadPageEventHandler(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoadGamePage(_storedGameBrowserViewModel));
    }

    private async void OpenSavePageEventHandler(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SaveGamePage(_storedGameBrowserViewModel));
    }

    public void NewGame(object? sender, NewGameEventArgs e)
    {
        _babaloneGameModel = new BabaloneModel(_babaloneDataAccess, e.BoardSize);
        _babaloneViewModel.LoadModel(_babaloneGameModel);
    }

    private async void LoadGameAsync(object? sender, StoredGameEventArgs e)
    {
        try
        {
            _babaloneGameModel = await _babaloneGameModel.LoadGameAsync(e.Path);
            _babaloneViewModel.LoadModel(_babaloneGameModel);
            await DisplayAlert("Success", $"Loaded game ({e.Path})", "OK");
        }
        catch (BabaloneDataException err)
        {
            await DisplayAlert("Error", $"Couldn't load game {e.Path} ({err.Message})", "OK");
        }
        await Navigation.PopToRootAsync();
    }

    private async void SaveGameAsync(object? sender, StoredGameEventArgs e)
    {
        try
        {
            await _babaloneGameModel.SaveGameAsync(e.Path);
            await _storedGameBrowserModel.UpdateAsync();
            await DisplayAlert("Success", $"Saved game as {e.Path}", "OK");
        }
        catch (BabaloneDataException err)
        {
            await DisplayAlert("Error", $"Couldn't save game as {e.Path} ({err.Message})", "OK");
        }
        await Navigation.PopToRootAsync();
    }
}
