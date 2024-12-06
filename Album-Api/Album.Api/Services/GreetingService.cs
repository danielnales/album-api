﻿using Microsoft.Extensions.Logging;
using System.Net;

namespace Album.Api.Services
{
    public interface IGreetingService
    {
        string GetGreeting(string name);
    }

    public class GreetingService : IGreetingService
    {
        private readonly ILogger<GreetingService> _logger;

        public GreetingService(ILogger<GreetingService> logger)
        {
            _logger = logger;
        }

        public string GetGreeting(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogInformation("Greeting requested with no name.");
                return "Hello, World!";
            }
            _logger.LogInformation("Greeting requested with name: {Name}", name);
            return $"Hello, {name} from {Dns.GetHostName()}v2!";
        }
    }
}
