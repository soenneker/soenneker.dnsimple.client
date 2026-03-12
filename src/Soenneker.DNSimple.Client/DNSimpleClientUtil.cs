using System;
using Microsoft.Extensions.Configuration;
using Soenneker.DNSimple.Client.Abstract;
using Soenneker.Extensions.Configuration;
using Soenneker.Utils.HttpClientCache.Abstract;
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

    private static readonly Uri _prodBaseUrl = new("https://api.dnsimple.com/v2/", UriKind.Absolute);
    private static readonly Uri _testBaseUrl = new("https://api.sandbox.dnsimple.com/v2/", UriKind.Absolute);
    private const string _clientId = nameof(DNSimpleClientUtil);

    public DNSimpleClientUtil(IHttpClientCache httpClientCache, IConfiguration configuration)
    {
        _httpClientCache = httpClientCache;
        _configuration = configuration;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        // No closure: state passed explicitly + static lambda
        return _httpClientCache.Get(_clientId, (configuration: _configuration, testBaseUrl: _testBaseUrl, prodBaseUrl: _prodBaseUrl), static state =>
        {
            var test = state.configuration.GetValueStrict<bool>("DNSimple:Test");
            Uri baseUrl = test ? state.testBaseUrl : state.prodBaseUrl;

            var token = state.configuration.GetValueStrict<string>("DNSimple:Token");

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
        _httpClientCache.RemoveSync(_clientId);
    }

    public ValueTask DisposeAsync()
    {
        return _httpClientCache.Remove(_clientId);
    }
}