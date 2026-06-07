using BlogCreator.Application.ViewModels;
using BlogCreator.Core.Interfaces;
using BlogCreator.Infrastructure.Services;
using BlogCreator.WinUI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading;
using WinRT.Interop;

namespace BlogCreator.WinUI;

public partial class App : Microsoft.UI.Xaml.Application
{
    public Window? MainWindow { get; private set; }
    public IHost Host { get; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IMarkdownRenderer, MarkdownRenderer>();
                services.AddSingleton<ShellViewModel>();
            })
            .Build();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        SynchronizationContext.SetSynchronizationContext(
            new DispatcherQueueSynchronizationContext(dispatcherQueue));

        await Host.StartAsync();

        MainWindow = new Window { Title = "BlogCreator" };
        var rootFrame = new Frame();
        rootFrame.Navigate(typeof(ShellPage));
        MainWindow.Content = rootFrame;

        AppWindow appWindow = GetAppWindow(MainWindow);
        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.Maximize();
        }

        MainWindow.Activate();
    }

    private static AppWindow GetAppWindow(Window window)
    {
        IntPtr hwnd = WindowNative.GetWindowHandle(window);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        return AppWindow.GetFromWindowId(windowId);
    }
}
