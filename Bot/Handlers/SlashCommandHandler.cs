using Bot.Services;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Bot.Handlers;

public class SlashCommandHandler (DiscordClientService discordClientService, SpotifyService spotifyService, ILogger<SlashCommandHandler> logger){
    private DiscordSocketClient client;

    public Task RegisterCommandHandlers() {
        client = discordClientService.GetClient();
        client.SlashCommandExecuted += HandleSlashCommand;
        return Task.CompletedTask;
    }

    private async Task HandleSlashCommand(SocketSlashCommand command) {
        switch (command.Data.Name) {
            case "login-spotify":
                logger.LogInformation("Received login-spotify command");
                await spotifyService.Connect();
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