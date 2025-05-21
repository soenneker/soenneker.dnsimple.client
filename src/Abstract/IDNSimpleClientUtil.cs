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
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
