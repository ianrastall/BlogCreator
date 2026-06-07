using BlogCreator.Core.Models;

namespace BlogCreator.Core.Interfaces;

public interface IPostRepository
{
    Task<IReadOnlyList<PostDocument>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(PostDocument post, CancellationToken cancellationToken = default);
}
