using EVAL.Babalone.ViewModel;

namespace EVAL.Babalone.View;

public partial class SaveGamePage : ContentPage
{
    public SaveGamePage(StoredGameBrowserViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
