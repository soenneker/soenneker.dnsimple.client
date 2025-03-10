using Microsoft.Extensions.Configuration;
using Soenneker.DNSimple.Client.Abstract;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.HttpClientCache.Dtos;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.DNSimple.Client;

/// <inheritdoc cref="IDNSimpleClientUtil"/>
public class DNSimpleClientUtil : IDNSimpleClientUtil
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;

    private const string _prodBaseUrl = "https://api.dnsimple.com/v2/";
    private const string _testBaseUrl = "https://api.sandbox.dnsimple.com/v2/";

    public DNSimpleClientUtil(IHttpClientCache httpClientCache, IConfiguration configuration)
    {
        _httpClientCache = httpClientCache;
        _configuration = configuration;
    }

    public ValueTask<HttpClient> Get(bool test = false, CancellationToken cancellationToken = default)
    {
        var token = _configuration.GetValueStrict<string>("DNSimple:Token");

        string baseUrl = test ? _testBaseUrl : _prodBaseUrl;

        var options = new HttpClientOptions
        {
            BaseAddress = baseUrl,
            DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", $"Bearer {token}" }
            }
        };

        string clientName = test ? $"{nameof(DNSimpleClientUtil)}-test" : nameof(DNSimpleClientUtil);

        return _httpClientCache.Get(clientName, options, cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _httpClientCache.RemoveSync(nameof(DNSimpleClientUtil));
        _httpClientCache.RemoveSync($"{nameof(DNSimpleClientUtil)}-test");
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _httpClientCache.Remove(nameof(DNSimpleClientUtil)).NoSync();
        await _httpClientCache.Remove($"{nameof(DNSimpleClientUtil)}-test").NoSync();
    }
}
