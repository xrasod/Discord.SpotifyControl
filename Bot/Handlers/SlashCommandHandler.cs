using Bot.Services;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Bot.Handlers;

public class SlashCommandHandler {
    private readonly SpotifyService _spotifyService;
    private readonly ILogger<SlashCommandHandler> _logger;
    private readonly DiscordSocketClient _client;
    public SlashCommandHandler(DiscordService discordService, SpotifyService spotifyService, ILogger<SlashCommandHandler> logger) {
        _spotifyService = spotifyService;
        _logger = logger;
        _client = discordService.GetClient();
        _client.SlashCommandExecuted += HandleSlashCommand;
    }

    private async Task HandleSlashCommand(SocketSlashCommand command) {
        switch (command.Data.Name) {
            case "login-spotify":
                _logger.LogInformation("Received login-spotify command");
                await _spotifyService.Connect();
                await command.RespondAsync("Successfully reauthorized with Spotify", ephemeral: true);
                break;
            case "play":
                // Handle play command
                break;
            case "enqueue":
                // Handle enqueue command
                break;
            case "skip":
                // Handle skip command
                break;
        }
    }


}