using Soenneker.DNSimple.Client.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.DNSimple.Client.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class DNSimpleClientUtilTests : HostedUnitTest
{
    private readonly IDNSimpleClientUtil _util;

    public DNSimpleClientUtilTests(Host host) : base(host)
    {
        _util = Resolve<IDNSimpleClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
