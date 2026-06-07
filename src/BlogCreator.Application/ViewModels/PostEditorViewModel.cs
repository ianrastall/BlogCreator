using BlogCreator.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text;

namespace BlogCreator.Application.ViewModels;

public partial class PostEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private Guid id = Guid.NewGuid();

    [ObservableProperty]
    private string title = "Untitled post";

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string slug = "untitled-post";

    [ObservableProperty]
    private string category = "Uncategorized";

    [ObservableProperty]
    private string tagsText = string.Empty;

    [ObservableProperty]
    private string body = "# Untitled post\n\nBegin writing here.";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StatusLabel))]
    private PostStatus status = PostStatus.Draft;

    [ObservableProperty]
    private DateTimeOffset? publishedAt;

    [ObservableProperty]
    private DateTimeOffset updatedAt = DateTimeOffset.Now;

    public string StatusLabel => Status.ToString();

    partial void OnTitleChanged(string value)
    {
        Slug = CreateSlug(value);
    }

    public PostDocument ToDocument()
    {
        return new PostDocument
        {
            Id = Id,
            Title = Title.Trim(),
            Description = Description.Trim(),
            Slug = Slug.Trim(),
            Category = Category.Trim(),
            Tags = TagsText
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            Body = Body,
            Status = Status,
            PublishedAt = PublishedAt,
            UpdatedAt = DateTimeOffset.Now
        };
    }

    private static string CreateSlug(string value)
    {
        var builder = new StringBuilder(value.Length);
        bool previousWasSeparator = false;

        foreach (char character in value.ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(character);
                previousWasSeparator = false;
            }
            else if (!previousWasSeparator && builder.Length > 0)
            {
                builder.Append('-');
                previousWasSeparator = true;
            }
        }

        return builder.ToString().Trim('-');
    }
}
