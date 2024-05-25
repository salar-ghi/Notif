using Application.Configuration;
using Elastic.Clients.Elasticsearch;

namespace Infrastructure.Configuration;

public class ElasticsearchService : IElasticsearchService
{
    private readonly ApplicationSettingExtenderModel _applicationSettingExtender;
    private readonly ElasticsearchClient _elasticsearchClient;    

    private readonly string _indexName;
    public ElasticsearchService(ElasticsearchClient elasticsearchClient, ApplicationSettingExtenderModel applicationSettingExtender)
    {
        _elasticsearchClient = elasticsearchClient;
        _applicationSettingExtender = applicationSettingExtender;
        _indexName = _applicationSettingExtender.ElasticSearch.IndexFormat;
    }

    public async Task SaveLogAsync<T>(T logEntry) where T : class
    {
        CancellationToken ct = default(CancellationToken);
        await _elasticsearchClient.IndexAsync<T>(logEntry, ct);

    }


    //public async Task<T> SearchAsync<T>(string query, string indexName) where T : class
    //{
    //    //var response = await _elasticsearchClient.GetAsync<T>(1, idx => idx
    //    //.Index("notif-log"));

    //    var searchResponse = await _elasticsearchClient.SearchAsync<T>(s => s
    //        .Index(indexName)
    //        .From(0)
    //        .Size(10)
    //        .Query(q => q
    //            .QueryString(qs => qs
    //                .Query(query)
    //            )
    //        )
    //    );

    //    if (searchResponse.IsValidResponse)
    //    {
    //        var tweet = searchResponse.Documents.FirstOrDefault();
    //    }

    //    return searchResponse;
    //}
}
