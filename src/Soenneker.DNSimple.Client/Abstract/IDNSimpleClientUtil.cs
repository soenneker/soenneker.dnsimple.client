using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.DNSimple.Client.Abstract;

/// <summary>
/// Provides a cached, authenticated <see cref="HttpClient"/> for DNSimple's API.
/// </summary>
public interface IDNSimpleClientUtil : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the client owned by this provider.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The configured DNSimple client.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
