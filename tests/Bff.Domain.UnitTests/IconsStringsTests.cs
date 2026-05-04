using Bff.Domain.Constants;
using Xunit;

namespace Bff.Domain.UnitTests;

public class IconsStringsTests
{
    [Fact]
    public void ShouldReturnRobotIcon()
    {
        Assert.Equal("🤖", IconsStrings.Robot);
    }

    [Fact]
    public void ShouldReturnIconByName()
    {
        var icons = new IconsStrings();
        Assert.Equal("🤖", icons["Robot"]);
    }
}
