using Microsoft.AspNetCore.Http;

using woc_backend_challenge.Handlers;
using woc_backend_challenge.Repositories;

namespace woc_backend_challenge.Tests.Handlers;

public class HitPointsHandlerTests
{
    private const string CharacterId = "test-character-id";

    private const string TestCharacterJson = """
    {
      "name": "TestCharacter",
      "level": 5,
      "hitPoints": 25,
      "classes": [],
      "stats": {
        "strength": 10, "dexterity": 10, "constitution": 10,
        "intelligence": 10, "wisdom": 10, "charisma": 10
      },
      "items": [],
      "defenses": []
    }
    """;

    private static CharacterRepository CreateRepository()
    {
        return new CharacterRepository(TestCharacterJson, CharacterId);
    }

    private static int GetStatusCode(IResult result)
    {
        return Assert.IsAssignableFrom<IStatusCodeHttpResult>(result).StatusCode!.Value;
    }

    private static dynamic GetValue(IResult result)
    {
        return Assert.IsAssignableFrom<IValueHttpResult>(result).Value!;
    }

    [Fact]
    public void PostDamage_ValidRequest_ReturnsOkConfirmation()
    {
        CharacterRepository repository = CreateRepository();
        var request = new HitPointsHandler.DamageRequest(14, "Piercing");

        IResult result = HitPointsHandler.PostDamage(CharacterId, request, repository);

        Assert.Equal(StatusCodes.Status200OK, GetStatusCode(result));
        dynamic value = GetValue(result);
        Assert.Equal("damage_applied", (string)value.message);
        Assert.Equal(CharacterId, (string)value.characterId);
        Assert.Equal(11, repository.Get(CharacterId).Character!.HitPoints);
    }

    [Fact]
    public void PostDamage_InvalidDamageType_ReturnsBadRequest()
    {
        CharacterRepository repository = CreateRepository();
        var request = new HitPointsHandler.DamageRequest(14, "not-a-real-type");

        IResult result = HitPointsHandler.PostDamage(CharacterId, request, repository);

        Assert.Equal(StatusCodes.Status400BadRequest, GetStatusCode(result));
    }

    [Fact]
    public void PostDamage_UnknownCharacter_ReturnsNotFound()
    {
        CharacterRepository repository = CreateRepository();
        var request = new HitPointsHandler.DamageRequest(14, "Piercing");

        IResult result = HitPointsHandler.PostDamage("unknown-id", request, repository);

        Assert.Equal(StatusCodes.Status404NotFound, GetStatusCode(result));
    }

    [Fact]
    public void PostHeal_ValidRequest_ReturnsOkConfirmation()
    {
        CharacterRepository repository = CreateRepository();
        repository.ApplyDamage(14, "Piercing", CharacterId); // HitPoints -> 11
        var request = new HitPointsHandler.HealRequest(5);

        IResult result = HitPointsHandler.PostHeal(CharacterId, request, repository);

        Assert.Equal(StatusCodes.Status200OK, GetStatusCode(result));
        dynamic value = GetValue(result);
        Assert.Equal("heal_applied", (string)value.message);
        Assert.Equal(16, repository.Get(CharacterId).Character!.HitPoints);
    }

    [Fact]
    public void PostHeal_NonPositivePoints_ReturnsBadRequest()
    {
        CharacterRepository repository = CreateRepository();
        var request = new HitPointsHandler.HealRequest(0);

        IResult result = HitPointsHandler.PostHeal(CharacterId, request, repository);

        Assert.Equal(StatusCodes.Status400BadRequest, GetStatusCode(result));
    }

    [Fact]
    public void PostHeal_UnknownCharacter_ReturnsNotFound()
    {
        CharacterRepository repository = CreateRepository();
        var request = new HitPointsHandler.HealRequest(5);

        IResult result = HitPointsHandler.PostHeal("unknown-id", request, repository);

        Assert.Equal(StatusCodes.Status404NotFound, GetStatusCode(result));
    }

    [Fact]
    public void PostTemporaryHeal_ValidRequest_ReturnsOkConfirmation()
    {
        CharacterRepository repository = CreateRepository();
        var request = new HitPointsHandler.TemporaryHitPointsRequest(10);

        IResult result = HitPointsHandler.PostTemporaryHeal(CharacterId, request, repository);

        Assert.Equal(StatusCodes.Status200OK, GetStatusCode(result));
        dynamic value = GetValue(result);
        Assert.Equal("temporary_hit_points_added", (string)value.message);
        Assert.Equal(10, repository.Get(CharacterId).Character!.TemporaryHitPoints);
    }

    [Fact]
    public void PostTemporaryHeal_NonPositivePoints_ReturnsBadRequest()
    {
        CharacterRepository repository = CreateRepository();
        var request = new HitPointsHandler.TemporaryHitPointsRequest(0);

        IResult result = HitPointsHandler.PostTemporaryHeal(CharacterId, request, repository);

        Assert.Equal(StatusCodes.Status400BadRequest, GetStatusCode(result));
    }

    [Fact]
    public void PostTemporaryHeal_UnknownCharacter_ReturnsNotFound()
    {
        CharacterRepository repository = CreateRepository();
        var request = new HitPointsHandler.TemporaryHitPointsRequest(10);

        IResult result = HitPointsHandler.PostTemporaryHeal("unknown-id", request, repository);

        Assert.Equal(StatusCodes.Status404NotFound, GetStatusCode(result));
    }
}
