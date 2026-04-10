using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomControls.Components;

public class PageItem(int pageNumber, bool isEllipsis, bool isCurrent)
{
    public int PageNumber { get; } = pageNumber;
    public bool IsEllipsis { get; } = isEllipsis;
    public bool IsCurrent { get; } = isCurrent;
    public string Display => IsEllipsis ? "..." : PageNumber.ToString();
}

public class Pagination : Control
{
    static Pagination()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(Pagination),
            new FrameworkPropertyMetadata(typeof(Pagination)));
    }

    public static readonly DependencyProperty TotalPagesProperty =
        DependencyProperty.Register(
            nameof(TotalPages),
            typeof(int),
            typeof(Pagination),
            new PropertyMetadata(1, OnPagingPropertyChanged, CoerceTotalPages));

    public int TotalPages
    {
        get => (int)GetValue(TotalPagesProperty);
        set => SetValue(TotalPagesProperty, value);
    }

    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(
            nameof(CurrentPage),
            typeof(int),
            typeof(Pagination),
            new FrameworkPropertyMetadata(
                1,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnPagingPropertyChanged,
                CoerceCurrentPage));

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public static readonly DependencyProperty DisplayCountProperty =
        DependencyProperty.Register(
            nameof(DisplayCount),
            typeof(int),
            typeof(Pagination),
            new PropertyMetadata(9, OnPagingPropertyChanged));

    public int DisplayCount
    {
        get => (int)GetValue(DisplayCountProperty);
        set => SetValue(DisplayCountProperty, value);
    }

    private static readonly DependencyPropertyKey PageItemsPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(PageItems),
            typeof(IReadOnlyList<PageItem>),
            typeof(Pagination),
            new PropertyMetadata(null));

    public static readonly DependencyProperty PageItemsProperty = PageItemsPropertyKey.DependencyProperty;

    public IReadOnlyList<PageItem> PageItems
    {
        get => (IReadOnlyList<PageItem>)GetValue(PageItemsProperty);
        private set => SetValue(PageItemsPropertyKey, value);
    }

    public static readonly RoutedEvent PageChangedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(PageChanged),
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<int>),
            typeof(Pagination));

    public event RoutedPropertyChangedEventHandler<int> PageChanged
    {
        add => AddHandler(PageChangedEvent, value);
        remove => RemoveHandler(PageChangedEvent, value);
    }

    public static readonly RoutedCommand PreviousPageCommand = new();
    public static readonly RoutedCommand NextPageCommand = new();
    public static readonly RoutedCommand GoToPageCommand = new();

    private bool _isUpdating;

    public Pagination()
    {
        CommandBindings.Add(new CommandBinding(PreviousPageCommand, OnPreviousPage, CanPreviousPage));
        CommandBindings.Add(new CommandBinding(NextPageCommand, OnNextPage, CanNextPage));
        CommandBindings.Add(new CommandBinding(GoToPageCommand, OnGoToPage));
        UpdatePageItems();
    }

    private static object CoerceTotalPages(DependencyObject d, object baseValue)
        => Math.Max(1, (int)baseValue);

    private static object CoerceCurrentPage(DependencyObject d, object baseValue)
    {
        var pagination = (Pagination)d;
        return Math.Clamp((int)baseValue, 1, pagination.TotalPages);
    }

    private static void OnPagingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var pagination = (Pagination)d;

        // Reentrancy guard: TotalPages → CoerceValue(CurrentPage) → OnPagingPropertyChanged 재진입 방지
        // UpdatePageItems를 1회만 실행하도록 보장
        if (pagination._isUpdating) return;

        try
        {
            pagination._isUpdating = true;

            if (e.Property == TotalPagesProperty)
                pagination.CoerceValue(CurrentPageProperty);

            pagination.UpdatePageItems();

            if (e.Property == CurrentPageProperty && (int)e.OldValue != (int)e.NewValue)
            {
                pagination.RaiseEvent(
                    new RoutedPropertyChangedEventArgs<int>(
                        (int)e.OldValue, (int)e.NewValue, PageChangedEvent));
            }
        }
        finally
        {
            pagination._isUpdating = false;
        }
    }

    private void UpdatePageItems()
    {
        int total = TotalPages;
        int current = CurrentPage;
        int display = DisplayCount;

        if (total <= 0)
        {
            PageItems = [];
            return;
        }

        // 새 List를 할당하여 단일 PropertyChanged로 ItemsControl 갱신
        // ObservableCollection.Clear+Add 방식 대비 레이아웃 무효화 1회로 감소
        var items = new List<PageItem>(display);

        if (total <= display)
        {
            for (int i = 1; i <= total; i++)
                items.Add(new PageItem(i, false, i == current));
        }
        else
        {
            int middleSlots = display - 4;
            int half = middleSlots / 2;

            if (current <= half + 2)
            {
                // Near start: 1,2,3,4,5,6,7,...,last
                for (int i = 1; i <= display - 2; i++)
                    items.Add(new PageItem(i, false, i == current));
                items.Add(new PageItem(0, true, false));
                items.Add(new PageItem(total, false, current == total));
            }
            else if (current >= total - half - 1)
            {
                // Near end: 1,...,last-6,last-5,...,last
                items.Add(new PageItem(1, false, current == 1));
                items.Add(new PageItem(0, true, false));
                for (int i = total - (display - 3); i <= total; i++)
                    items.Add(new PageItem(i, false, i == current));
            }
            else
            {
                // Middle: 1,...,cur-2,cur-1,cur,cur+1,cur+2,...,last
                items.Add(new PageItem(1, false, false));
                items.Add(new PageItem(0, true, false));
                for (int i = current - half; i <= current + half; i++)
                    items.Add(new PageItem(i, false, i == current));
                items.Add(new PageItem(0, true, false));
                items.Add(new PageItem(total, false, false));
            }
        }

        PageItems = items;
    }

    private void CanPreviousPage(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = CurrentPage > 1;

    private void OnPreviousPage(object sender, ExecutedRoutedEventArgs e)
        => CurrentPage--;

    private void CanNextPage(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = CurrentPage < TotalPages;

    private void OnNextPage(object sender, ExecutedRoutedEventArgs e)
        => CurrentPage++;

    private void OnGoToPage(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is int page)
            CurrentPage = page;
        else if (e.Parameter is string s && int.TryParse(s, out int parsed))
            CurrentPage = parsed;
    }
}
