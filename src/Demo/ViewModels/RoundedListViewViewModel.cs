using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Demo.ViewModels;

public partial class RoundedListViewViewModel : ObservableObject
{
    public ObservableCollection<BoxItem> Items { get; } =
    [
        new("2501001", "A 창고", "대기 중"),
        new("2501002", "B 창고", "작업 중"),
        new("25010013", "C 창고", "완료"),
    ];
}

public record BoxItem(string BoxNumber, string Location, string BoxStatus);
