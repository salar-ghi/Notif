using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public record ApplicationSettingModel
    {
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public ElasticSearchConfiguration ElasticSearch { get; set; }
        public ServiceLocatorConfiguration ServiceLocator { get; set; }
    }

    public record ServiceLocatorConfiguration
    {
        public IEnumerable<string> DLLPaths { get; set; }

    }

    public record ElasticSearchConfiguration
    {
        public string Host { get; set; }
        public string IndexFormat { get; set; }

    }

    public record RabbitMQConfiguration
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }


}
