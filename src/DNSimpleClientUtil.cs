using Microsoft.Extensions.Configuration;
using Soenneker.DNSimple.Client.Abstract;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.HttpClientCache.Abstract;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Dtos.HttpClientOptions;

namespace Soenneker.DNSimple.Client;

/// <inheritdoc cref="IDNSimpleClientUtil"/>
public sealed class DNSimpleClientUtil : IDNSimpleClientUtil
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;

    private const string _prodBaseUrl = "https://api.dnsimple.com/v2/";
    private const string _testBaseUrl = "https://api.sandbox.dnsimple.com/v2/";
    private const string _clientId = nameof(DNSimpleClientUtil);
    private const string _testClientId = nameof(DNSimpleClientUtil) + "-test";

    public DNSimpleClientUtil(IHttpClientCache httpClientCache, IConfiguration configuration)
    {
        _httpClientCache = httpClientCache;
        _configuration = configuration;
    }

    public ValueTask<HttpClient> Get(bool test = false, CancellationToken cancellationToken = default)
    {
        string clientId = test ? _testClientId : _clientId;
        string baseUrl = test ? _testBaseUrl : _prodBaseUrl;

        return _httpClientCache.Get(clientId, () =>
        {
            var token = _configuration.GetValueStrict<string>("DNSimple:Token");

            return new HttpClientOptions
            {
                BaseAddress = baseUrl,
                DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token}" }
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClientCache.RemoveSync(_clientId);
        _httpClientCache.RemoveSync(_testClientId);
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _httpClientCache.Remove(_clientId).NoSync();
        await _httpClientCache.Remove(_testClientId).NoSync();
    }
}
