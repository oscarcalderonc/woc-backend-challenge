using Microsoft.AspNetCore.Mvc;
using woc_backend_challenge.Repositories;

namespace woc_backend_challenge.Handlers;

public static class HitPointsHandler
{
    public record DamageRequest(int HitPoints, string DamageType);
    public record HealRequest(int HealPoints);
    public record TemporaryHitPointsRequest(int TemporaryHitPoints);

    public static IResult PostDamage(
        [FromRoute] string characterId,
        DamageRequest request,
        CharacterRepository repository)
    {
        ApplyDamageResult result = repository.ApplyDamage(request.HitPoints, request.DamageType, characterId);

        return result.Error switch
        {
            ApplyDamageError.InvalidHitPoints => Results.BadRequest(new
            {
                error = "invalid_hit_points",
                hitPoints = request.HitPoints,
            }),
            ApplyDamageError.InvalidDamageType => Results.BadRequest(new
            {
                error = "invalid_damage_type",
                damageType = request.DamageType,
            }),
            ApplyDamageError.CharacterNotFound => Results.NotFound(new
            {
                error = "character_not_found",
                characterId,
            }),
            _ => Results.Ok(new
            {
                message = "damage_applied",
                characterId,
                hitPoints = request.HitPoints,
                damageType = request.DamageType,
            }),
        };
    }

    public static IResult PostHeal(
        [FromRoute] string characterId,
        HealRequest request,
        CharacterRepository repository)
    {
        HealResult result = repository.Heal(request.HealPoints, characterId);

        return result.Error switch
        {
            HealError.InvalidHealPoints => Results.BadRequest(new
            {
                error = "invalid_heal_points",
                healPoints = request.HealPoints,
            }),
            HealError.CharacterNotFound => Results.NotFound(new
            {
                error = "character_not_found",
                characterId,
            }),
            _ => Results.Ok(new
            {
                message = "heal_applied",
                characterId,
                healPoints = request.HealPoints,
            }),
        };
    }

    public static IResult PostTemporaryHeal(
        [FromRoute] string characterId,
        TemporaryHitPointsRequest request,
        CharacterRepository repository)
    {
        TemporaryHitPointsResult result = repository.AddTemporaryHitPoints(request.TemporaryHitPoints, characterId);

        return result.Error switch
        {
            TemporaryHitPointsError.InvalidTemporaryHitPoints => Results.BadRequest(new
            {
                error = "invalid_temporary_hit_points",
                temporaryHitPoints = request.TemporaryHitPoints,
            }),
            TemporaryHitPointsError.CharacterNotFound => Results.NotFound(new
            {
                error = "character_not_found",
                characterId,
            }),
            _ => Results.Ok(new
            {
                message = "temporary_hit_points_added",
                characterId,
                temporaryHitPoints = request.TemporaryHitPoints,
            }),
        };
    }
}
