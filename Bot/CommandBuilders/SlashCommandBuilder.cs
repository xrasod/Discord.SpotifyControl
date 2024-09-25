using Bot.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Bot.CommandBuilders;

public class SlashCommandBuilder {
    private readonly ILogger<SlashCommandBuilder> _logger;
    private readonly DiscordSocketClient _client;


    public SlashCommandBuilder(DiscordService discordService, ILogger<SlashCommandBuilder> logger) {
        _logger = logger;
        _client = discordService.GetClient();
        RegisterCommands().GetAwaiter().GetResult();
    }

    private SlashCommandProperties BuildLoginCommand() =>
        new Discord.SlashCommandBuilder()
            .WithName("login-spotify")
            .WithDescription("Reauthorizes the bot with Spotify")
            .AddOption("secret", ApplicationCommandOptionType.String, "A secret phrase for reauthorization so you don't spam me. Nice try idiots.", isRequired: true)
            .Build();

    private SlashCommandProperties BuildPlayCommand() =>
        new Discord.SlashCommandBuilder()
            .WithName("play")
            .WithDescription("Plays a song from Spotify immidiately")
            .AddOption("song", ApplicationCommandOptionType.String, "Song id", isRequired: true)
            .Build();

    private SlashCommandProperties BuildEnqueueCommand() =>
        new Discord.SlashCommandBuilder()
            .WithName("enqueue")
            .WithDescription("Enqueues a song from Spotify")
            .AddOption("song", ApplicationCommandOptionType.String, "Song id", isRequired: true)
            .Build();

    private SlashCommandProperties BuildSkipCommand() =>
        new Discord.SlashCommandBuilder()
            .WithName("skip")
            .WithDescription("Skips the current song")
            .Build();

    // Register commands
    public async Task RegisterCommands() {
        try {
            await _client.CreateGlobalApplicationCommandAsync(BuildLoginCommand());
            await _client.CreateGlobalApplicationCommandAsync(BuildPlayCommand());
            await _client.CreateGlobalApplicationCommandAsync(BuildEnqueueCommand());
            await _client.CreateGlobalApplicationCommandAsync(BuildSkipCommand());

        }
        catch (Exception e) {
            _logger.LogError(e, "Could not register slash commands");
            throw;
        }
    }
}