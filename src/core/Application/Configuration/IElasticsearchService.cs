namespace Application.Configuration;

public interface IElasticsearchService
{
    Task SaveLogAsync<T>(T logEntry) where T : class;
}
