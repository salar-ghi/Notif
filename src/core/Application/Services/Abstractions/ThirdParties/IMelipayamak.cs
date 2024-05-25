namespace Application.Services.Abstractions.ThirdParties;

public interface IMelipayamak : ISmsProvider
{
    Task SendMelipayamakSmsAsync(Message message);
}
