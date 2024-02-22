using EVAL.Babalone.Persistence;

namespace EVAL.Babalone;

public partial class App : Application
{
    private const string SuspendedGameSavePath = "SuspendedBabaloneGame.baba";

    private readonly AppShell _appShell;

    public App()
    {
        InitializeComponent();

        _appShell = new AppShell(new BabaloneFileDataAccess());

        MainPage = _appShell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Deactivated += (_, _) =>
            Task.Run(() => _appShell.ViewModel.SaveGameAsync(SuspendedGameSavePath));

        window.Activated += (_, _) =>
            Task.Run(() => _appShell.ViewModel.LoadGameAsync(SuspendedGameSavePath));

        return window;
    }
}
