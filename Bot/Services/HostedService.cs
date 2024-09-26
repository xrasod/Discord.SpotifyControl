using Bot.CommandBuilders;
using Bot.Configuration;
using Bot.Handlers;
using Bot.Util;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bot.Services;

public class HostedService(
    DiscordClientService discordClientService,
    ILogger<HostedService> logger,
    SlashCommandBuilder slashCommandBuilder,
    SlashCommandHandler slashCommandHandler
    )
    : IHostedService {

    public async Task StartAsync(CancellationToken cancellationToken) {
        await discordClientService.Connect();
        await slashCommandBuilder.RegisterCommands();
        await slashCommandHandler.RegisterCommandHandlers();
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return discordClientService.Disconnect();
    }
}