using Discord;
using Microsoft.Extensions.Logging;

namespace Bot.Util;

public class DiscordLogger(ILogger<DiscordLogger> logger) {
    public Task Log(LogMessage msg) {

        switch (msg.Severity) {
            case LogSeverity.Debug:
                logger.LogDebug(msg.Message, msg.Source);
                break;
            case LogSeverity.Info:
                logger.LogInformation(msg.Message, msg.Source);
                break;
            case LogSeverity.Warning:
                logger.LogWarning(msg.Message, msg.Source);
                break;
            case LogSeverity.Error:
                logger.LogError(msg.Exception, msg.Message, msg.Source);
                break;
            case LogSeverity.Critical:
                logger.LogCritical(msg.Exception, msg.Message, msg.Source);
                break;
            case LogSeverity.Verbose:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        return Task.CompletedTask;
    }
}