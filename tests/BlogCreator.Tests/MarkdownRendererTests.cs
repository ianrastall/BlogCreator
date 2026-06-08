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

    [Fact]
    public void RenderDocument_IncludesPreviewContentSecurityPolicy()
    {
        var renderer = new MarkdownRenderer();

        string html = renderer.RenderDocument("<script>alert('preview')</script>");

        Assert.Contains("Content-Security-Policy", html, StringComparison.Ordinal);
        Assert.Contains("script-src 'none'", html, StringComparison.Ordinal);
    }
}
