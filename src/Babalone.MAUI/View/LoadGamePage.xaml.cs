using EVAL.Babalone.ViewModel;

namespace EVAL.Babalone.View;

public partial class LoadGamePage : ContentPage
{
    public LoadGamePage(StoredGameBrowserViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
