namespace Application.Services.Abstractions.ThirdParties;

public interface IIdehpardazan : ISmsProvider
{
    Task SendIdehpardazSmsAsync(Message message);
}
