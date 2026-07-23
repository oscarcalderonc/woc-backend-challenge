using Microsoft.AspNetCore.Http;

using woc_backend_challenge.Handlers;

namespace woc_backend_challenge.Tests.Handlers;

public class HealthzHandlerTests
{
    [Fact]
    public void Get_ReturnsOkWithPassStatus()
    {
        IResult result = HealthzHandler.Get();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, statusResult.StatusCode);

        IValueHttpResult valueResult = Assert.IsAssignableFrom<IValueHttpResult>(result);
        dynamic value = valueResult.Value!;
        Assert.Equal("pass", (string)value.status);
    }
}
