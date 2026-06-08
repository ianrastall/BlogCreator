using BlogCreator.Application.ViewModels;
using BlogCreator.Infrastructure.Services;

namespace BlogCreator.Tests;

public sealed class ShellViewModelTests
{
    [Fact]
    public void Constructor_CreatesWelcomePostAndPreview()
    {
        var viewModel = new ShellViewModel(new MarkdownRenderer());

        Assert.Single(viewModel.Posts);
        Assert.NotNull(viewModel.SelectedPost);
        Assert.Contains("Welcome to BlogCreator", viewModel.PreviewHtml, StringComparison.Ordinal);
    }

    [Fact]
    public void NewPostCommand_AddsAndSelectsDraft()
    {
        var viewModel = new ShellViewModel(new MarkdownRenderer());

        viewModel.NewPostCommand.Execute(null);

        Assert.Equal(2, viewModel.Posts.Count);
        Assert.Equal("Untitled post", viewModel.SelectedPost?.Title);
    }

    [Fact]
    public void SelectedPostChange_NotifiesCommandAvailability()
    {
        var viewModel = new ShellViewModel(new MarkdownRenderer());
        var originalPost = viewModel.SelectedPost;
        int saveDraftNotifications = 0;
        int publishNotifications = 0;

        viewModel.SaveDraftCommand.CanExecuteChanged += (_, _) => saveDraftNotifications++;
        viewModel.PublishCommand.CanExecuteChanged += (_, _) => publishNotifications++;

        viewModel.SelectedPost = null;

        Assert.False(viewModel.SaveDraftCommand.CanExecute(null));
        Assert.False(viewModel.PublishCommand.CanExecute(null));

        viewModel.SelectedPost = originalPost;

        Assert.True(viewModel.SaveDraftCommand.CanExecute(null));
        Assert.True(viewModel.PublishCommand.CanExecute(null));
        Assert.Equal(2, saveDraftNotifications);
        Assert.Equal(2, publishNotifications);
    }
}
