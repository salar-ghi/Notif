using Domain.Entities;
using Hangfire.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.EntityFramework;

public class MessageManagementService : IMessageManagementService
{
    #region Definition & Ctor
    private readonly ILogger<MessageManagementService> _logger;
    private readonly IMapper _mapper;

    private readonly IMessageService _message;
    private readonly IProviderService _provider;
    private readonly IMessageLogService _messageLog;
    private readonly ICacheMessage _cache;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageSender _messageSender;

    public MessageManagementService(ILogger<MessageManagementService> logger, IMapper mapper, IProviderService provider,
        IMessageService message, IMessageLogService messageLog, ICacheMessage cache, IServiceProvider serviceProvider,
        IMessageSender messageSender)
    {
        _logger = logger;
        _mapper = mapper;
        _message = message;
        _provider = provider;
        _messageLog = messageLog;
        _cache = cache;
        _serviceProvider = serviceProvider;
        _messageSender = messageSender;
    }
    #endregion

    #region Methods

    public async Task<bool> SaveMessagesToStorage(CancellationToken ct = default(CancellationToken))
    {
        try
        {
            var entities = await _cache.GetAllMessages();
            if (!entities.IsNullOrEmpty())
            {
                await _message.SaveMessageAsync(entities, ct);
                await _cache.RemoveMessage(entities.ToList());
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    public ISmsProvider GetService(string ProviderName)
    {
        try
        {
            return ProviderName switch
            {
                "MeliPayamak" => _serviceProvider.GetRequiredService<Melipayamak>(),
                "Idehpardazan" => _serviceProvider.GetRequiredService<Idehpardazan>(),
                "PayamSms" => _serviceProvider.GetRequiredService<PayamSms>(),
                _ => throw new KeyNotFoundException("Provider not found.")
            };
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.Message, ex);
            throw;
        }
    }

    public async Task<bool> SendMessages(CancellationToken ct = default(CancellationToken))
    {
        try
        {
            var notifs = await _message.GetUnDeliveredAsync();
            Provider provider = new Provider();
            foreach (var item in notifs)
            {
                if (item.ProviderID > 0)
                {
                    provider = await _provider.GetSpecificProvider(item.ProviderID, item.Type);
                }
                else
                {
                    provider = await _provider.GetRandomProvider(item.Type);
                }
                var NotifSender = await _messageSender.ManageMessage(provider, item, ct);
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
        finally
        {

        }

    }

    #endregion
}
