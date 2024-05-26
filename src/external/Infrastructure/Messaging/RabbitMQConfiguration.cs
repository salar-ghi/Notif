using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public class RabbitMQConfiguration
{
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;

    
    //private readonly string _hostName;
    //private readonly string _channelPrefix;
    //private readonly string _messagePrefix;
    private readonly ILogger _logger;

    public RabbitMQConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IModel GetChannel()
    {
        if (_channel != null && _channel.IsOpen)
            return _channel;

        var factory = new ConnectionFactory
        {
            HostName = _configuration.GetSection("RabbitMQ").GetSection("HostName").Value,  //["RabbitMQ:HostName"],
            UserName = _configuration["RabbitMQ:UserName"],
            Password = _configuration["RabbitMQ:Password"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        return _channel;
    }

}
