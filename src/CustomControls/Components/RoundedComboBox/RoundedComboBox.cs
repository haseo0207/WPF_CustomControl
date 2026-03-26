using System.Windows;
using System.Windows.Controls;

namespace CustomControls.Components;

public class RoundedComboBox : ComboBox
{
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(RoundedComboBox),
            new FrameworkPropertyMetadata(new CornerRadius(6)));

    public static readonly DependencyProperty PopupCornerRadiusProperty =
        DependencyProperty.Register(
            nameof(PopupCornerRadius),
            typeof(CornerRadius),
            typeof(RoundedComboBox),
            new FrameworkPropertyMetadata(new CornerRadius(6)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public CornerRadius PopupCornerRadius
    {
        get => (CornerRadius)GetValue(PopupCornerRadiusProperty);
        set => SetValue(PopupCornerRadiusProperty, value);
    }

    static RoundedComboBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(RoundedComboBox),
            new FrameworkPropertyMetadata(typeof(RoundedComboBox)));
    }
}
