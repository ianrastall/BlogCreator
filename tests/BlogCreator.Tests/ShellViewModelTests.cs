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
}
