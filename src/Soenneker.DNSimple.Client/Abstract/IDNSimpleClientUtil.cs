using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.DNSimple.Client.Abstract;

/// <summary>
/// An async thread-safe singleton for the DNSimple client
/// </summary>
public interface IDNSimpleClientUtil : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
