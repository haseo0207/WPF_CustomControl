using System.Windows;
using System.Windows.Controls;

namespace CustomControls.Components;

public class RoundedCheckBox : CheckBox
{
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(RoundedCheckBox),
            new FrameworkPropertyMetadata(new CornerRadius(4)));

    public static readonly DependencyProperty BoxSizeProperty =
        DependencyProperty.Register(
            nameof(BoxSize),
            typeof(double),
            typeof(RoundedCheckBox),
            new FrameworkPropertyMetadata(20.0));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public double BoxSize
    {
        get => (double)GetValue(BoxSizeProperty);
        set => SetValue(BoxSizeProperty, value);
    }

    static RoundedCheckBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(RoundedCheckBox),
            new FrameworkPropertyMetadata(typeof(RoundedCheckBox)));
    }
}
