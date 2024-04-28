using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace WebCore.Messaging.Rabbitmq
{
    public static class MassTransitExtensions
    {
        public static Task<ISendEndpoint> GetSendEndpoint(this ISendEndpointProvider provider, string exchangeName = "", string queueName = "")
        {
            var text = "";
            if (!string.IsNullOrWhiteSpace(exchangeName)) text = $"exchange: {exchangeName}";
            if (!string.IsNullOrWhiteSpace(exchangeName) && !string.IsNullOrWhiteSpace(queueName)) text += "?bind=true&";
            if (!string.IsNullOrWhiteSpace(queueName)) text += $"queue: {queueName}";

            return provider.GetSendEndpoint(new Uri(text));
        }

    }


    class Class1 //: IEndpointDefinition
    {
        ISendEndpoint sendBus;
        ISendEndpointProvider sendProvider;
        IPublishEndpoint publishBus;
        public Class1()
        {
            //sendBus.Send("", (context)=> context.SetRoutingKey(""));
            var endpoint = sendProvider.GetSendEndpoint(exchangeName: "my exchange", queueName: "my queue").Result;
            endpoint.Send("", (context) => context.SetRoutingKey(""));
            //publishBus.Publish("", (context) => context.SetRoutingKey(""),);

    }
}


}
