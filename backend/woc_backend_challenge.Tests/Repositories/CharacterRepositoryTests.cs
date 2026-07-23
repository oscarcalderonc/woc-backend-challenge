using woc_backend_challenge.Models;
using woc_backend_challenge.Repositories;

namespace woc_backend_challenge.Tests.Repositories;

public class CharacterRepositoryTests
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
      "defenses": [
        { "type": "fire", "defense": "immunity" },
        { "type": "slashing", "defense": "resistance" }
      ]
    }
    """;

    private static CharacterRepository CreateRepository()
    {
        return new CharacterRepository(TestCharacterJson, CharacterId);
    }

    [Fact]
    public void Constructor_WithExplicitCharacterId_StoresCharacterUnderThatId()
    {
        CharacterRepository repository = CreateRepository();

        CharacterResult result = repository.Get(CharacterId);

        Assert.Equal(CharacterError.None, result.Error);
        Assert.Equal(CharacterId, result.Character!.Id);
    }

    [Fact]
    public void ApplyDamage_NoResistanceOrImmunity_SubtractsFullDamage()
    {
        CharacterRepository repository = CreateRepository();

        ApplyDamageResult result = repository.ApplyDamage(14, "Piercing", CharacterId);

        Assert.Equal(ApplyDamageError.None, result.Error);
        Assert.Equal(11, result.Character!.HitPoints);
    }

    [Fact]
    public void ApplyDamage_DamageTypeIsCaseInsensitive()
    {
        CharacterRepository repository = CreateRepository();

        ApplyDamageResult result = repository.ApplyDamage(14, "piercing", CharacterId);

        Assert.Equal(ApplyDamageError.None, result.Error);
        Assert.Equal(11, result.Character!.HitPoints);
    }

    [Fact]
    public void ApplyDamage_Resistance_HalvesDamage()
    {
        CharacterRepository repository = CreateRepository();

        ApplyDamageResult result = repository.ApplyDamage(20, "Slashing", CharacterId);

        Assert.Equal(ApplyDamageError.None, result.Error);
        Assert.Equal(15, result.Character!.HitPoints); // 25 - (20 * 0.5)
    }

    [Fact]
    public void ApplyDamage_Immunity_NegatesDamage()
    {
        CharacterRepository repository = CreateRepository();

        ApplyDamageResult result = repository.ApplyDamage(20, "Fire", CharacterId);

        Assert.Equal(ApplyDamageError.None, result.Error);
        Assert.Equal(25, result.Character!.HitPoints);
    }

    [Fact]
    public void ApplyDamage_InvalidDamageType_ReturnsError()
    {
        CharacterRepository repository = CreateRepository();

        ApplyDamageResult result = repository.ApplyDamage(10, "not-a-real-type", CharacterId);

        Assert.Equal(ApplyDamageError.InvalidDamageType, result.Error);
        Assert.Null(result.Character);
    }

    [Fact]
    public void ApplyDamage_UnknownCharacter_ReturnsError()
    {
        CharacterRepository repository = CreateRepository();

        ApplyDamageResult result = repository.ApplyDamage(10, "Fire", "unknown-id");

        Assert.Equal(ApplyDamageError.CharacterNotFound, result.Error);
        Assert.Null(result.Character);
    }

    [Fact]
    public void Heal_ValidPoints_IncreasesHitPoints()
    {
        CharacterRepository repository = CreateRepository();
        repository.ApplyDamage(14, "Piercing", CharacterId); // HitPoints -> 11

        HealResult result = repository.Heal(5, CharacterId);

        Assert.Equal(HealError.None, result.Error);
        Assert.Equal(16, result.Character!.HitPoints);
    }

    [Fact]
    public void Heal_NonPositivePoints_ReturnsError()
    {
        CharacterRepository repository = CreateRepository();

        HealResult result = repository.Heal(0, CharacterId);

        Assert.Equal(HealError.InvalidHealPoints, result.Error);
    }

    [Fact]
    public void Heal_UnknownCharacter_ReturnsError()
    {
        CharacterRepository repository = CreateRepository();

        HealResult result = repository.Heal(5, "unknown-id");

        Assert.Equal(HealError.CharacterNotFound, result.Error);
    }

    [Fact]
    public void Get_KnownCharacter_ReturnsCharacter()
    {
        CharacterRepository repository = CreateRepository();

        CharacterResult result = repository.Get(CharacterId);

        Assert.Equal(CharacterError.None, result.Error);
        Assert.Equal(CharacterId, result.Character!.Id);
    }

    [Fact]
    public void Get_UnknownCharacter_ReturnsError()
    {
        CharacterRepository repository = CreateRepository();

        CharacterResult result = repository.Get("unknown-id");

        Assert.Equal(CharacterError.CharacterNotFound, result.Error);
        Assert.Null(result.Character);
    }

    [Fact]
    public void AddTemporaryHitPoints_ValidPoints_UpdatesCharacter()
    {
        CharacterRepository repository = CreateRepository();

        TemporaryHitPointsResult result = repository.AddTemporaryHitPoints(10, CharacterId);

        Assert.Equal(TemporaryHitPointsError.None, result.Error);
        Assert.Equal(10, result.Character!.TemporaryHitPoints);
    }

    [Fact]
    public void AddTemporaryHitPoints_NonPositivePoints_ReturnsError()
    {
        CharacterRepository repository = CreateRepository();

        TemporaryHitPointsResult result = repository.AddTemporaryHitPoints(0, CharacterId);

        Assert.Equal(TemporaryHitPointsError.InvalidTemporaryHitPoints, result.Error);
    }

    [Fact]
    public void AddTemporaryHitPoints_UnknownCharacter_ReturnsError()
    {
        CharacterRepository repository = CreateRepository();

        TemporaryHitPointsResult result = repository.AddTemporaryHitPoints(10, "unknown-id");

        Assert.Equal(TemporaryHitPointsError.CharacterNotFound, result.Error);
    }
}
