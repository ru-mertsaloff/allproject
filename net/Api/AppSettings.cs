using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Api
{
    public class AppSettings
    {
        public string RabbitMQHost { get; set; }

        public string RabbitMQVirtualHost { get; set; }

        public string RabbitMQLogin { get; set; }

        public string RabbitMQPassword { get; set; }

        public Dictionary<string, LogLevel> ServicesMinLevels { get; set; }

    }

}
