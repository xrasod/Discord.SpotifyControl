using Bot.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;

namespace Bot.Services;

public class SpotifyService {
    private readonly Secrets _secrets;
    private readonly ILogger<SpotifyService> _logger;
    private string _accessToken;
    private SpotifyClient _spotifyClient;
    public SpotifyService(IOptions<Secrets> secretsOptions, ILogger<SpotifyService> logger) {
        _secrets = secretsOptions.Value;
        _logger = logger;
    }

    public async Task Connect() {
        _logger.LogInformation("Starting Spotify service");
        await GetAccessToken();
        _spotifyClient = new SpotifyClient(_accessToken);
        _logger.LogInformation("Successfully authenticated and started spotify client for user {User}", _accessToken);
    }

    public async Task<string> QueueSong(string songUri) {

        var trackId = songUri.Split("/").Last();
        if(trackId.Contains('?')) {
            trackId = trackId.Split("?").First();
        }
        var track = await _spotifyClient.Tracks.Get(trackId);

        await _spotifyClient.Player.AddToQueue(new PlayerAddToQueueRequest(track.Uri));
        return $"Queued {track.Name} by {track.Artists.First().Name}";
    }

    private async Task GetAccessToken() {
        try {
            SpotifyAuthService spotifyAuthService = new(_secrets.SpotifyClientId, _secrets.SpotifyClientSecret);
            _accessToken = await spotifyAuthService.Authorize();

        }
        catch (Exception e) {
            _logger.LogError(e, "Could not authenticate with Spotify API");
            throw;
        }
    }
}