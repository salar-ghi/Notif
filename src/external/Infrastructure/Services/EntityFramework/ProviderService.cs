namespace Infrastructure.Services.EntityFramework;

public class ProviderService : CRUDService<Provider>, IProviderService
{
    #region Definition & Ctor
    private readonly ILogger<ProviderService> _logger;
    private readonly IMapper _mapper;
    public ProviderService(ILogger<ProviderService> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<Provider> GetSpecificProvider(string name)
    {
        try
        {
            var provider = await _unitOfWork.DbContext.Providers
                .Where(z => z.IsEnabled == true && z.Name == name)
                .AsNoTracking()
                .Select(j => new Provider { 
                    Id = j.Id,
                    Name = j.Name,
                    Type = j.Type,
                    JsonConfig = j.JsonConfig
                })
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
            return provider;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
    public async Task<Provider> GetRandomProvider(string? name, NotifType? type)
    {
        try
        {
            //var providername = _mapper.Map<ProviderType>(type.ToString());
            var providerType = _mapper.Map<ProviderType>(type);
            ProviderType prType = (ProviderType)Enum.Parse(typeof(ProviderType), providerType.ToString(), true);

            var ranProvider = await _unitOfWork.DbContext.Providers
                //.AsParallel()
                //.WithDegreeOfParallelism(Environment.ProcessorCount)
                .Where(z => z.Type == prType && z.IsEnabled == true && z.Priority == 1)
                .AsNoTracking()
                .Select(j => new Provider
                {
                    Id = j.Id,
                    Name = j.Name,
                    Type = j.Type,
                    JsonConfig = j.JsonConfig
                })
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
            return ranProvider;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    public Task<Provider> GetProvider(string name)
    {
        throw new NotImplementedException();
    }

    //public async Task<Provider> GetProvider(string name)
    //{
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(name))
    //        {
    //            var providerId = await _unitOfWork.DbContext.Providers
    //                .Where(z => z.IsEnabled == true && z.Name == name)
    //                .AsNoTracking()
    //                .Select(j => j.Id)
    //                .SingleOrDefaultAsync()
    //                .ConfigureAwait(false);
    //            // get provider Id by provioder name
    //            notLog.ProviderId = providerId;
    //        }
    //        else
    //        {
    //            var targetEnum = (ProviderType)Enum.Parse(typeof(ProviderType), entity.Type.ToString());
    //            var providername = _mapper.Map<ProviderType>(entity.Type);

    //            var ranProviderId = await _unitOfWork.DbContext.Providers
    //                .Where(z => z.Type == providername && z.IsEnabled == true && z.Priority == 1)
    //                .AsNoTracking()
    //                .Select(j => j.Id)
    //                .FirstOrDefaultAsync().ConfigureAwait(false);
    //            notLog.ProviderId = ranProviderId;

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex.Message, ex);
    //        throw;
    //    }
    //}







    #endregion
}
