using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Noticebord.Client.Models;

namespace Noticebord.Client
{

    public class NoticebordClient : IClient
    {
        private const string DefaultBaseUrl = "https://noticebord.herokuapp.com/api";

        public bool IsLoggedIn => Token is not null;
        public string BaseUrl { get; }
        public string? Token { get; }

        public NoticebordClient(string? token, string? baseUrl = default)
        {
            Token = token;
            BaseUrl = baseUrl ?? DefaultBaseUrl;
        }

        public async Task<string> AuthenticateAsync(
            AuthenticateRequest request,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegment("tokens")
                .PostJsonAsync(request, cancellationToken)
                .ReceiveString();

        public async Task<Notice> CreateNoticeAsync(
            SaveNoticeRequest request,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegment("notices")
                .WithHeader("Authorization", IsLoggedIn ? string.Empty : $"Bearer {Token}")
                .PostJsonAsync(request, cancellationToken)
                .ReceiveJson<Notice>();

        public async Task<Notice> GetNoticeAsync(
            long id,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegments("notices", id)
                .GetJsonAsync<Notice>(cancellationToken);

        public async Task<List<Notice>> GetNoticesAsync(
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegment("notices")
                .GetJsonAsync<List<Notice>>(cancellationToken);

        public async Task<Notice> UpdateNoticeAsync(
            long id,
            SaveNoticeRequest request,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegments("notices", id)
                .WithHeader("Authorization", IsLoggedIn ? string.Empty : $"Bearer {Token}")
                .PutJsonAsync(request, cancellationToken)
                .ReceiveJson<Notice>();

        public async Task DeleteNoticeAsync(
            long id,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegments("notices", id)
                .WithHeader("Authorization", IsLoggedIn ? string.Empty : $"Bearer {Token}")
                .DeleteAsync(cancellationToken);

        public async Task<Notice> CreateTeamNoticeAsync(
            long team,
            SaveTeamNoticeRequest request,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegments("teams", team, "notices")
                .WithHeader("Authorization", IsLoggedIn ? string.Empty : $"Bearer {Token}")
                .PostJsonAsync(request, cancellationToken)
                .ReceiveJson<Notice>();

        public async Task<Notice> GetTeamNoticeAsync(
            long team,
            long id,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegments("teams", team, "notices", id)
                .WithHeader("Authorization", IsLoggedIn ? string.Empty : $"Bearer {Token}")
                .GetJsonAsync<Notice>(cancellationToken);

        public async Task<List<Notice>> GetTeamNoticesAsync(
            long team,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegments("teams", team, "notices")
                .WithHeader("Authorization", IsLoggedIn ? string.Empty : $"Bearer {Token}")
                .GetJsonAsync<List<Notice>>(cancellationToken);

        public async Task<Notice> UpdateTeamNoticeAsync(
            long team,
            long id,
            SaveTeamNoticeRequest request,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegments("teams", team, "notices", id)
                .WithHeader("Authorization", IsLoggedIn ? string.Empty : $"Bearer {Token}")
                .PutJsonAsync(request, cancellationToken)
                .ReceiveJson<Notice>();

        public async Task DeleteTeamNoticeAsync(
            long team,
            long id,
            CancellationToken cancellationToken = default) =>
            await BaseUrl.AppendPathSegments("teams", team, "notices", id)
                .WithHeader("Authorization", IsLoggedIn ? string.Empty : $"Bearer {Token}")
                .DeleteAsync(cancellationToken);
    }
}