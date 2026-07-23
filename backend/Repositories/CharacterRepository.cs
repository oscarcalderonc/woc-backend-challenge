using System.Text.Json.Serialization;
using System.Text.Json;

using woc_backend_challenge.Models;

namespace woc_backend_challenge.Repositories;

public enum ApplyDamageError
{
    None,
    InvalidDamageType,
    InvalidHitPoints,
    CharacterNotFound
}

public enum HealError
{
    None,
    InvalidHealPoints,
    CharacterNotFound
}

public enum TemporaryHitPointsError
{
    None,
    InvalidTemporaryHitPoints,
    CharacterNotFound
}
public enum CharacterError
{
    None,
    CharacterNotFound
}



public record ApplyDamageResult(Character? Character, ApplyDamageError Error);
public record HealResult(Character? Character, HealError Error);
public record TemporaryHitPointsResult(Character? Character, TemporaryHitPointsError Error);
public record CharacterResult(Character? Character, CharacterError Error);

public class CharacterRepository
{

    public CharacterRepository(string json, string? characterId = null)
    {
        characters = new();
        Character firstCharacter = JsonSerializer.Deserialize<Character>(json) ?? new Character();
        if (!string.IsNullOrEmpty(characterId))
        {
            firstCharacter.Id = characterId;
        }
        characters.Add(firstCharacter.Id, firstCharacter);
    }

    public ApplyDamageResult ApplyDamage(int hitPoints, string typeOfDamage, string characterId)
    {
        if (hitPoints <= 0)
        {
            return new ApplyDamageResult(null, ApplyDamageError.InvalidHitPoints);
        }
        if (!Enum.TryParse<DamageType>(typeOfDamage, ignoreCase: true, out DamageType result))
        {
            return new ApplyDamageResult(null, ApplyDamageError.InvalidDamageType);
        }
        if (!this.characters.TryGetValue(characterId, out Character? character))
        {
            return new ApplyDamageResult(null, ApplyDamageError.CharacterNotFound);
        }
        CharacterDefense? defense = character.GetCharacterDefense(result);
        double finalBlow = hitPoints;
        if (defense != null)
        {
            switch (defense.Defense)
            {
                case DefenseType.Resistance:
                    finalBlow *= 0.5;
                    break;
                case DefenseType.Immunity:
                    finalBlow *= 0;
                    break;
            }
        }

        character.DecreaseHitPoints((int)Math.Round(finalBlow));
        return new ApplyDamageResult(character, ApplyDamageError.None);
    }


    public HealResult Heal(int healPoints, string characterId)
    {
        if (healPoints <= 0)
        {
            return new HealResult(null, HealError.InvalidHealPoints);
        }

        if (!this.characters.TryGetValue(characterId, out Character? character))
        {
            return new HealResult(null, HealError.CharacterNotFound);
        }

        character.IncreaseHitPoints(healPoints);
        return new HealResult(character, HealError.None);
    }

    public CharacterResult Get(string characterId)
    {
        if (!this.characters.TryGetValue(characterId, out Character? character))
        {
            return new CharacterResult(null, CharacterError.CharacterNotFound);
        }

        return new CharacterResult(character, CharacterError.None);
    }

    public TemporaryHitPointsResult AddTemporaryHitPoints(int temporaryHitPoints, string characterId)
    {
        if (temporaryHitPoints <= 0)
        {
            return new TemporaryHitPointsResult(null, TemporaryHitPointsError.InvalidTemporaryHitPoints);
        }

        if (!this.characters.TryGetValue(characterId, out Character? character))
        {
            return new TemporaryHitPointsResult(null, TemporaryHitPointsError.CharacterNotFound);
        }

        character.AddTemporaryHitPoints(temporaryHitPoints);
        return new TemporaryHitPointsResult(character, TemporaryHitPointsError.None);
    }

    private readonly Dictionary<string, Character> characters;

}
