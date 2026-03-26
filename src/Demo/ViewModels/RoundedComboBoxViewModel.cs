using CommunityToolkit.Mvvm.ComponentModel;

namespace Demo.ViewModels;

public partial class RoundedComboBoxViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _selectedItem;

    public List<string> Items { get; } =
    [
        "-",
        "領収書",
        "請求書",
        "契約書",
        "納品書",
        "注文書"
    ];
}
