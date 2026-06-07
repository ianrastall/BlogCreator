using BlogCreator.Application.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace BlogCreator.WinUI.Views;

public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel { get; }

    public ShellPage()
    {
        InitializeComponent();

        ViewModel = ((App)Microsoft.UI.Xaml.Application.Current).Host.Services.GetRequiredService<ShellViewModel>();
        DataContext = ViewModel;

        Loaded += ShellPage_Loaded;
        Unloaded += ShellPage_Unloaded;
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private async void ShellPage_Loaded(object sender, RoutedEventArgs e)
    {
        await RefreshPreviewAsync();
    }

    private void ShellPage_Unloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
    }

    private async void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ShellViewModel.PreviewHtml))
        {
            await RefreshPreviewAsync();
        }
    }

    private async Task RefreshPreviewAsync()
    {
        await PreviewWebView.EnsureCoreWebView2Async();
        PreviewWebView.NavigateToString(ViewModel.PreviewHtml);
    }
}
