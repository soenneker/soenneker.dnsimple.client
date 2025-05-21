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

    public DNSimpleClientUtil(IHttpClientCache httpClientCache, IConfiguration configuration)
    {
        _httpClientCache = httpClientCache;
        _configuration = configuration;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(_clientId, () =>
        {
            var test = _configuration.GetValueStrict<bool>("DNSimple:Test");
            string baseUrl = test ? _testBaseUrl : _prodBaseUrl;

            var token = _configuration.GetValueStrict<string>("DNSimple:Token");

            return new HttpClientOptions
            {
                BaseAddress = baseUrl,
                DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
                {
                    {"Authorization", $"Bearer {token}"}
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClientCache.RemoveSync(_clientId);
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _httpClientCache.Remove(_clientId).NoSync();
    }
}