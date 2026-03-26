using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CustomControls.Components;

public sealed class BorderClipConverter : IMultiValueConverter
{
    public static readonly BorderClipConverter Instance = new();

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 3
            || values[0] is not double width
            || values[1] is not double height
            || values[2] is not CornerRadius radius)
        {
            return DependencyProperty.UnsetValue;
        }

        if (width < double.Epsilon || height < double.Epsilon)
            return Geometry.Empty;

        var rect = new Rect(0, 0, width, height);
        var geometry = new RectangleGeometry(rect, radius.TopLeft, radius.TopLeft);
        geometry.Freeze();
        return geometry;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
