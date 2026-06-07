namespace BlogCreator.Core.Interfaces;

public interface IMarkdownRenderer
{
    string RenderDocument(string markdown);
}
