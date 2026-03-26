# WPF CustomControl Project

## Tech Stack
- .NET 10, WPF
- CommunityToolkit.Mvvm (MVVM Toolkit)
- C# 13

## Documentation
- MS 공식 문서를 기준으로 개발: https://learn.microsoft.com/dotnet/desktop/wpf/
- CommunityToolkit.Mvvm: https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/

## Project Structure
- 각 컴포넌트는 독립 폴더로 관리
- 컴포넌트별 View와 1:1 매핑으로 개별 조회 가능

```
src/
  CustomControls/          # CustomControl 라이브러리 프로젝트
    Components/
      {ComponentName}/
        {ComponentName}.cs           # CustomControl 본체
        {ComponentName}.xaml         # Generic.xaml 내 Style (or 개별 ResourceDictionary)
  Demo/                    # Demo 앱 프로젝트
    Views/
      {ComponentName}View.xaml       # 컴포넌트별 1:1 데모 View
      {ComponentName}View.xaml.cs
    ViewModels/
      {ComponentName}ViewModel.cs
```

## Conventions
- CustomControl은 Parts and States Model 패턴 준수
- DependencyProperty로 바인딩 가능한 속성 정의
- Demo ViewModel은 CommunityToolkit.Mvvm의 ObservableObject, RelayCommand, ObservableProperty 활용
- XAML 스타일은 Generic.xaml 또는 컴포넌트별 ResourceDictionary로 관리
