using Microsoft.AspNetCore.Mvc;
using woc_backend_challenge.Repositories;

namespace woc_backend_challenge.Handlers;

public static class CharacterHandler
{

    public static IResult Get(
        [FromRoute] string characterId,
        CharacterRepository repository)
    {
        CharacterResult result = repository.Get(characterId);

        return result.Error switch
        {
            CharacterError.CharacterNotFound => Results.NotFound(new
            {
                error = "character_not_found",
                characterId,
            }),
            _ => Results.Ok(result.Character),
        };
    }
}
