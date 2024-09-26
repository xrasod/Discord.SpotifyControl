using Bot.CommandBuilders;
using Bot.Configuration;
using Bot.Handlers;
using Bot.ServiceProviders;
using Bot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Bot;

internal class Program {
    private static async Task Main(string[] args) {
        var host = CreateHostBuilder(args);
        await host.RunAsync();
        

    }

    private static IHost CreateHostBuilder(string[] args) {
        var builder = Host.CreateApplicationBuilder(args);
        var env = builder.Environment;

        builder.Logging.AddSimpleConsole(logging => {
            logging.SingleLine = true;
            logging.TimestampFormat = "[HH:mm:ss] ";
            logging.ColorBehavior = LoggerColorBehavior.Enabled;
        });

        builder.Configuration.Sources.Clear();
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();

        builder.Services.Configure<SpotifyControlOptions>(
            builder.Configuration.GetSection(key: nameof(SpotifyControlOptions))
        );
        builder.Services.Configure<Secrets>(
            builder.Configuration.GetSection(key: nameof(Secrets))
        );


        builder.Services
            .AddHostedService<HostedService>()
            .AddSingleton<DiscordClientService>()
            .AddSingleton<SpotifyService>()
            .AddDiscordNetService()
            .AddSingleton<SlashCommandBuilder>()
            .AddSingleton<SlashCommandHandler>();
        return builder.Build();
    }
}