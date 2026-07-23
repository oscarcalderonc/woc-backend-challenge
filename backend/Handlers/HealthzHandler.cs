namespace woc_backend_challenge.Handlers;

public static class HealthzHandler
{
    public static IResult Get() => Results.Ok(new {
      status = "pass",
      version= "0.1",
      uptime= 86400,
    });
}
