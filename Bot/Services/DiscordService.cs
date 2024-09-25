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
    private readonly Secrets _secretsOptions;
    private readonly ILogger<DiscordService> _logger;
    private readonly SpotifyService _spotifyService;

    public DiscordService(IOptions<Secrets> secretsOptions, ILogger<DiscordService> logger, DiscordLogger discordDiscordLogger, SpotifyService spotifyService) {
        _secretsOptions = secretsOptions.Value;
        _logger = logger;
        _spotifyService = spotifyService;
        _client = new DiscordSocketClient();
        _client.Log += discordDiscordLogger.Log;
    }

    private async Task<Task> Connect() {
        await _client.LoginAsync(TokenType.Bot, _secretsOptions.Token);
        await _client.StartAsync();
        await _spotifyService.Connect();
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