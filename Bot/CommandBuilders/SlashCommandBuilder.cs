using Bot.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Bot.CommandBuilders;

public class SlashCommandBuilder(DiscordClientService discordClientService, ILogger<SlashCommandBuilder> logger) {
    private readonly DiscordSocketClient _client = discordClientService.GetClient();

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
            var guild = _client.GetGuild(1029766905717850122);


            await guild.CreateApplicationCommandAsync(BuildLoginCommand());
            await guild.CreateApplicationCommandAsync(BuildPlayCommand());
            await guild.CreateApplicationCommandAsync(BuildEnqueueCommand());
            await guild.CreateApplicationCommandAsync(BuildSkipCommand());

        }
        catch (Exception e) {
            logger.LogError(e, "Could not register slash commands");
            throw;
        }
    }
}