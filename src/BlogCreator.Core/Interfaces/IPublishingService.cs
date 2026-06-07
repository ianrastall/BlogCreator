using BlogCreator.Core.Models;

namespace BlogCreator.Core.Interfaces;

public interface IPublishingService
{
    Task PublishAsync(PostDocument post, CancellationToken cancellationToken = default);
}
