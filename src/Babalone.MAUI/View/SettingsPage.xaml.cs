using EVAL.Babalone.ViewModel;

namespace EVAL.Babalone.View;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsPageViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
