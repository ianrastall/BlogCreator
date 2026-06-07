using BlogCreator.Infrastructure.Services;

namespace BlogCreator.Tests;

public sealed class MarkdownRendererTests
{
    [Fact]
    public void RenderDocument_ConvertsHeadingAndFencedCode()
    {
        var renderer = new MarkdownRenderer();

        string html = renderer.RenderDocument("# Heading\n\n```csharp\nint answer = 42;\n```");

        Assert.Contains("<h1", html, StringComparison.Ordinal);
        Assert.Contains("language-csharp", html, StringComparison.Ordinal);
        Assert.Contains("int answer = 42;", html, StringComparison.Ordinal);
    }
}
