using Bot.Configuration;
using Bot.Util;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bot.Services;

public class HostedService : IHostedService{
    private readonly ILogger<HostedService> _logger;
    private readonly DiscordService _discordService;

    public HostedService(DiscordService discordService, ILogger<HostedService> logger) {
        _logger = logger;
        _discordService = discordService;
    }
    public Task StartAsync(CancellationToken cancellationToken) {
        return _discordService.Connect();
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return _discordService.Disconnect();
    }
}