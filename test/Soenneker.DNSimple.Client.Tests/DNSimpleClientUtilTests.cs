using Soenneker.DNSimple.Client.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.DNSimple.Client.Tests;

[Collection("Collection")]
public class DNSimpleClientUtilTests : FixturedUnitTest
{
    private readonly IDNSimpleClientUtil _util;

    public DNSimpleClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IDNSimpleClientUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
