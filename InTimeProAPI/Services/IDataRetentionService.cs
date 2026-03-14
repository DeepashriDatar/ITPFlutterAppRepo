namespace InTimeProAPI.Services;

public interface IDataRetentionService
{
    Task ApplyRetentionAsync(CancellationToken cancellationToken);
}
