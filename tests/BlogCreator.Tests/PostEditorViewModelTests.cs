using BlogCreator.Application.ViewModels;

namespace BlogCreator.Tests;

public sealed class PostEditorViewModelTests
{
    [Theory]
    [InlineData("Hello, World!", "hello-world")]
    [InlineData("!!!", "untitled-post")]
    [InlineData("", "untitled-post")]
    public void TitleChanged_CreatesStableSlug(string title, string expectedSlug)
    {
        var viewModel = new PostEditorViewModel();

        viewModel.Title = title;

        Assert.Equal(expectedSlug, viewModel.Slug);
    }

    [Fact]
    public void ToDocument_TrimsMetadataAndTags()
    {
        var viewModel = new PostEditorViewModel
        {
            Title = "  My Post  ",
            Description = "  Description  ",
            Slug = "  my-post  ",
            Category = "  Essays  ",
            TagsText = " one, two ,, three "
        };

        var document = viewModel.ToDocument();

        Assert.Equal("My Post", document.Title);
        Assert.Equal("Description", document.Description);
        Assert.Equal("my-post", document.Slug);
        Assert.Equal("Essays", document.Category);
        Assert.Equal(["one", "two", "three"], document.Tags);
    }
}
