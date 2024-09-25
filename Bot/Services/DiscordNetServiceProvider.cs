﻿using Bot.Util;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Services;

public static class DiscordNetServiceProvider {
    public static IServiceCollection AddDiscordNetService(this IServiceCollection services) {
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<DiscordLogger>();
        return services;
    }
}