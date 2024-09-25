using Bot.Handlers;
using Bot.Util;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SlashCommandBuilder = Bot.CommandBuilders.SlashCommandBuilder;

namespace Bot.ServiceProviders;

public static class DiscordNetServiceProvider {
    public static IServiceCollection AddDiscordNetService(this IServiceCollection services) {
        services
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<DiscordLogger>();
        return services;
    }
}