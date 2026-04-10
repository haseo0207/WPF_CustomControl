using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Demo.ViewModels;

public partial class PaginationViewModel : ObservableObject
{
    private const int TotalItemCount = 237;

    [ObservableProperty]
    private int _itemsPerPage = 10;

    [ObservableProperty]
    private int _totalPages;

    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private ObservableCollection<string> _pageItems = [];

    public PaginationViewModel()
    {
        UpdateTotalPages();
        UpdatePageItems();
    }

    partial void OnItemsPerPageChanged(int value)
    {
        UpdateTotalPages();

        if (CurrentPage > TotalPages)
            CurrentPage = TotalPages;
        else
            UpdatePageItems();
    }

    partial void OnCurrentPageChanged(int value)
    {
        UpdatePageItems();
    }

    private void UpdateTotalPages()
    {
        TotalPages = (int)Math.Ceiling((double)TotalItemCount / Math.Max(1, ItemsPerPage));
    }

    private void UpdatePageItems()
    {
        PageItems.Clear();

        int start = (CurrentPage - 1) * ItemsPerPage + 1;
        int end = Math.Min(start + ItemsPerPage - 1, TotalItemCount);

        for (int i = start; i <= end; i++)
            PageItems.Add($"Item #{i}");
    }
}
