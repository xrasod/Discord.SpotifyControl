using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;

namespace Bot.Services;

public class SpotifyAuthService(string clientId, string clientSecret) {
    private readonly EmbedIOAuthServer _server = new(new Uri("http://localhost:5543/spotifycallback"), 5543);
    private TaskCompletionSource<string> _tcs;

    private async Task Initialize() {
        await _server.Start();
        _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;
    }

    public async Task<string> Authorize() {
        _tcs = new TaskCompletionSource<string>();
        await Initialize();

        var request = new LoginRequest(_server.BaseUri, clientId, LoginRequest.ResponseType.Code) {
            Scope = new[] {
                Scopes.UserReadCurrentlyPlaying,
                Scopes.UserReadEmail,
                Scopes.UserReadPlaybackState,
                Scopes.UserModifyPlaybackState,
            }
        };

        BrowserUtil.Open(request.ToUri());
        return await _tcs.Task;
    }

    private async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response) {
        await _server.Stop();

        var config = SpotifyClientConfig.CreateDefault();
        var tokenResponse = await new OAuthClient(config).RequestToken(
            new AuthorizationCodeTokenRequest(clientId, clientSecret, response.Code, new Uri("http://localhost:5543/spotifycallback"))
        );
        _tcs.SetResult(tokenResponse.AccessToken);
    }

    private async Task OnErrorRecieved(object sender, string error, string state) {
        await _server.Stop();
        _tcs.SetException(new Exception(error));
    }
}