[![](https://img.shields.io/nuget/v/soenneker.dnsimple.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.dnsimple.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.dnsimple.client/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.dnsimple.client/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.dnsimple.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.dnsimple.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.dnsimple.client/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.dnsimple.client/actions/workflows/codeql.yml)

# Soenneker.DNSimple.Client

Provides a cached `HttpClient` configured for DNSimple's production or sandbox API with bearer-token authentication.

## Installation

```bash
dotnet add package Soenneker.DNSimple.Client
```

## Configuration

```json
{
  "DNSimple": {
    "Token": "your-api-token",
    "Test": false
  }
}
```

Set `DNSimple:Test` to `true` to use `https://api.sandbox.dnsimple.com/v2/`; otherwise the client uses `https://api.dnsimple.com/v2/`. The token is sent as `Authorization: Bearer <Token>`.

## Registration and usage

```csharp
using Soenneker.DNSimple.Client.Abstract;
using Soenneker.DNSimple.Client.Registrars;

services.AddDNSimpleClientUtilAsSingleton();

public sealed class DNSimpleIdentityReader(IDNSimpleClientUtil clients)
{
    public async Task<HttpResponseMessage> Get(CancellationToken cancellationToken)
    {
        HttpClient client = await clients.Get(cancellationToken);
        return await client.GetAsync("whoami", cancellationToken);
    }
}
```

The provider owns the returned client. Singleton registration keeps one client for the application lifetime. Scoped registration creates a separately owned cache entry per scope, so disposing one scope cannot remove another provider's client.
