using Bot.Configuration;
using Bot.Util;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bot.Services;

public class DiscordClientService {
    private readonly DiscordSocketClient _client;
    private readonly Secrets _secretsOptions;
    private readonly ILogger<DiscordClientService> _logger;
    private readonly SpotifyService _spotifyService;

    public DiscordClientService(IOptions<Secrets> secretsOptions, ILogger<DiscordClientService> logger, DiscordLogger discordDiscordLogger, SpotifyService spotifyService) {
        _secretsOptions = secretsOptions.Value;
        _logger = logger;
        _spotifyService = spotifyService;

        _client = new DiscordSocketClient();
        _client.Log += discordDiscordLogger.Log;
    }

    public DiscordSocketClient GetClient() {
        return _client;
    }

    public async Task<Task> Connect() {
        _logger.LogInformation("Starting Discord client");

        await _client.LoginAsync(TokenType.Bot, _secretsOptions.Token);
        await _client.StartAsync();
        // await _spotifyService.Connect();
        return Task.CompletedTask;
    }


    public async Task<Task> Disconnect() {
        _logger.LogInformation("Stopping Discord client");
        await _client.StopAsync();
        return Task.CompletedTask;
    }


}