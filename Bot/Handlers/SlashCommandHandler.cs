using Bot.Configuration;
using Bot.Services;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bot.Handlers;

public class SlashCommandHandler {
    private DiscordSocketClient client;
    private readonly DiscordClientService _discordClientService;
    private readonly SpotifyService _spotifyService;
    private readonly ILogger<SlashCommandHandler> _logger;
    private readonly SpotifyControlOptions _options;

    public SlashCommandHandler(DiscordClientService discordClientService, SpotifyService spotifyService, ILogger<SlashCommandHandler> logger, IOptions<SpotifyControlOptions> options) {
        _discordClientService = discordClientService;
        _spotifyService = spotifyService;
        _logger = logger;
        _options = options.Value;
    }

    public Task RegisterCommandHandlers() {
        client = _discordClientService.GetClient();
        client.SlashCommandExecuted += HandleSlashCommand;
        return Task.CompletedTask;
    }

    private async Task HandleSlashCommand(SocketSlashCommand command) {
        switch (command.Data.Name) {
            case "login-spotify":
                await HandleLoginSpotifyCommand(command);
                break;
            case "play":
                // Handle play command
                break;
            case "enqueue":
                await HandleEnqueueCommand(command);
                break;
            case "skip":
                // Handle skip command
                break;
        }
    }

    private async Task HandleLoginSpotifyCommand(SocketSlashCommand command) {
        if(command.Data.Options.First().Value.ToString() != _options.SpotifyAuthSecret) {
            await command.RespondAsync("You need to provide a secret phrase to reauthorize with Spotify", ephemeral: true);
            return;
        }
        _logger.LogInformation("Received login-spotify command");
        await _spotifyService.Connect();
        await command.RespondAsync("Successfully reauthorized with Spotify", ephemeral: true);
    }

    private async Task HandleEnqueueCommand(SocketSlashCommand command) {
        var songUri = command.Data.Options.First().Value.ToString();
        if (string.IsNullOrWhiteSpace(songUri)) throw new ArgumentException();
        var result = await _spotifyService.QueueSong(songUri);
        await command.RespondAsync(result);
    }
}