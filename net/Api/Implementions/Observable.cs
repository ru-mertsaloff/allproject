using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Api.Implementions
{
    public class Observable : IObservable
    {
        private readonly ILogger _logger;
        private readonly AppSettings _configuration;

        public Observable(ILogger<Observable> logger, IOptions<AppSettings> options)
        {
            _logger = logger;
            _configuration = options.Value;
        }

        public void HandlerLog(Message message)
        {
            try
            {
                _logger.LogInformation($"Received Text: {message}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "упс");
                throw;
            }
        }
    }
}