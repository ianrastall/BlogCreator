namespace BlogCreator.Core.Models;

public sealed class PostDocument
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public IReadOnlyList<string> Tags { get; set; } = [];
    public string Body { get; set; } = string.Empty;
    public PostStatus Status { get; set; } = PostStatus.Draft;
    public DateTimeOffset? PublishedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
}
