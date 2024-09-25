using Bot.Configuration;
using Bot.Util;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bot.Services;

public class DiscordService : IHostedService {
    private readonly DiscordSocketClient _client;
    private readonly SpotifyControlOptions _options;
    private readonly ILogger<DiscordService> _logger;

    public DiscordService(IOptions<SpotifyControlOptions> options, ILogger<DiscordService> logger, DiscordLogger discordDiscordLogger) {
        _options = options.Value;
        _logger = logger;
        _client = new DiscordSocketClient();
        _client.Log += discordDiscordLogger.Log;
    }

    private async Task<Task> Connect() {
        await _client.LoginAsync(TokenType.Bot, _options.Token);
        await _client.StartAsync();
        return Task.CompletedTask;
    }

    private async Task<Task> Disconnect() {
        await _client.StopAsync();
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Starting Discord client");
        return Connect();
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Stopping Discord client");
        return Disconnect();
    }
}