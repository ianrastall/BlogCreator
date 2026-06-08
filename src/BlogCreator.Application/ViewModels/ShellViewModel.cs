using BlogCreator.Core.Interfaces;
using BlogCreator.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BlogCreator.Application.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    private readonly IMarkdownRenderer markdownRenderer;

    public ShellViewModel(IMarkdownRenderer markdownRenderer)
    {
        this.markdownRenderer = markdownRenderer;

        var welcomePost = new PostEditorViewModel
        {
            Title = "Welcome to BlogCreator",
            Description = "The first BlogCreator workspace document.",
            Category = "BlogCreator",
            TagsText = "welcome, setup",
            Body = "# Welcome to BlogCreator\n\nThis first milestone establishes the WinUI 3 publishing workspace.\n\n```csharp\nConsole.WriteLine(\"Markdown code blocks are preserved.\");\n```"
        };

        Posts.Add(welcomePost);
        SelectedPost = welcomePost;
        StatusMessage = "Application scaffold loaded. Draft persistence and publishing are not connected yet.";
    }

    public ObservableCollection<PostEditorViewModel> Posts { get; } = [];

    [ObservableProperty]
    private PostEditorViewModel? selectedPost;

    [ObservableProperty]
    private string previewHtml = string.Empty;

    [ObservableProperty]
    private string statusMessage = "Ready.";

    partial void OnSelectedPostChanged(PostEditorViewModel? oldValue, PostEditorViewModel? newValue)
    {
        if (oldValue is not null)
        {
            oldValue.PropertyChanged -= SelectedPost_PropertyChanged;
        }

        if (newValue is not null)
        {
            newValue.PropertyChanged += SelectedPost_PropertyChanged;
        }

        SaveDraftCommand.NotifyCanExecuteChanged();
        PublishCommand.NotifyCanExecuteChanged();
        RefreshPreview();
    }

    [RelayCommand]
    private void NewPost()
    {
        var post = new PostEditorViewModel();
        Posts.Insert(0, post);
        SelectedPost = post;
        StatusMessage = "New in-memory draft created.";
    }

    [RelayCommand(CanExecute = nameof(HasSelectedPost))]
    private void SaveDraft()
    {
        if (SelectedPost is null)
        {
            return;
        }

        SelectedPost.Status = PostStatus.Draft;
        SelectedPost.UpdatedAt = DateTimeOffset.Now;
        StatusMessage = "Draft persistence is the next implementation milestone; nothing has been written to disk yet.";
    }

    [RelayCommand(CanExecute = nameof(HasSelectedPost))]
    private void Publish()
    {
        StatusMessage = "Publishing is intentionally not connected in the bootstrap milestone.";
    }

    [RelayCommand]
    private void RefreshPreview()
    {
        PreviewHtml = markdownRenderer.RenderDocument(SelectedPost?.Body ?? string.Empty);
    }

    private bool HasSelectedPost() => SelectedPost is not null;

    private void SelectedPost_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PostEditorViewModel.Body))
        {
            RefreshPreview();
        }

        SaveDraftCommand.NotifyCanExecuteChanged();
        PublishCommand.NotifyCanExecuteChanged();
    }
}
