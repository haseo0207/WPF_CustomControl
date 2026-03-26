using System.Windows;
using System.Windows.Controls;
using Demo.Views;

namespace Demo;

public partial class MainWindow : Window
{
    private readonly RoundedComboBoxView _roundedComboBoxView = new();
    private readonly ScrollBarView _scrollBarView = new();

    public MainWindow()
    {
        InitializeComponent();
        ContentArea.Content = _roundedComboBoxView;
    }

    private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ContentArea is null) return;

        ContentArea.Content = NavList.SelectedIndex switch
        {
            0 => _roundedComboBoxView,
            1 => _scrollBarView,
            _ => null
        };
    }
}
