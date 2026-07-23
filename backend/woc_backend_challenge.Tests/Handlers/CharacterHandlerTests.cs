using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

using woc_backend_challenge.Handlers;
using woc_backend_challenge.Models;
using woc_backend_challenge.Repositories;

namespace woc_backend_challenge.Tests.Handlers;

public class CharacterHandlerTests
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

    [Fact]
    public void Get_KnownCharacter_ReturnsOkWithCharacter()
    {
        CharacterRepository repository = CreateRepository();

        IResult result = CharacterHandler.Get(CharacterId, repository);

        Ok<Character> ok = Assert.IsType<Ok<Character>>(result);
        Assert.Equal(CharacterId, ok.Value!.Id);
    }

    [Fact]
    public void Get_UnknownCharacter_ReturnsNotFound()
    {
        CharacterRepository repository = CreateRepository();

        IResult result = CharacterHandler.Get("unknown-id", repository);

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, statusResult.StatusCode);
    }
}
