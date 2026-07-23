using woc_backend_challenge.Models;

namespace woc_backend_challenge.Tests.Models;

public class CharacterTests
{
    private static Character CreateCharacter(int hitPoints, int temporaryHitPoints = 0)
    {
        return new Character
        {
            HitPoints = hitPoints,
            TemporaryHitPoints = temporaryHitPoints,
        };
    }

    [Fact]
    public void GetCharacterDefense_MatchingType_ReturnsDefense()
    {
        Character character = CreateCharacter(25);
        character.Defenses.Add(new CharacterDefense { Type = DamageType.Fire, Defense = DefenseType.Immunity });

        CharacterDefense? defense = character.GetCharacterDefense(DamageType.Fire);

        Assert.NotNull(defense);
        Assert.Equal(DefenseType.Immunity, defense!.Defense);
    }

    [Fact]
    public void GetCharacterDefense_NoMatchingType_ReturnsNull()
    {
        Character character = CreateCharacter(25);
        character.Defenses.Add(new CharacterDefense { Type = DamageType.Fire, Defense = DefenseType.Immunity });

        CharacterDefense? defense = character.GetCharacterDefense(DamageType.Cold);

        Assert.Null(defense);
    }

    [Fact]
    public void GetTotalHitPoints_SumsBaseAndTemporary()
    {
        Character character = CreateCharacter(25, temporaryHitPoints: 10);

        Assert.Equal(35, character.GetTotalHitPoints());
    }

    [Fact]
    public void DecreaseHitPoints_NoTemporaryHitPoints_SubtractsFromHitPoints()
    {
        // From CHALLENGE.md's example: 25 HP, 14 Piercing damage -> 11 HP
        Character character = CreateCharacter(25);

        character.DecreaseHitPoints(14);

        Assert.Equal(11, character.HitPoints);
        Assert.Equal(CharacterStatus.Alive, character.Status);
    }

    [Fact]
    public void DecreaseHitPoints_DamageExceedsTemporaryHitPoints_DepletesTempThenHitPoints()
    {
        // From CHALLENGE.md's Eldric example: 11 HP, 10 temp HP, hit for 19 -> 2 HP, 0 temp HP
        Character character = CreateCharacter(11, temporaryHitPoints: 10);

        character.DecreaseHitPoints(19);

        Assert.Equal(2, character.HitPoints);
        Assert.Equal(0, character.TemporaryHitPoints);
        Assert.Equal(CharacterStatus.Alive, character.Status);
    }

    [Fact]
    public void DecreaseHitPoints_DamageLessThanTemporaryHitPoints_OnlyReducesTemp()
    {
        Character character = CreateCharacter(25, temporaryHitPoints: 10);

        character.DecreaseHitPoints(4);

        Assert.Equal(25, character.HitPoints);
        Assert.Equal(6, character.TemporaryHitPoints);
    }

    [Fact]
    public void DecreaseHitPoints_DamageMeetsOrExceedsTotal_KillsCharacter()
    {
        Character character = CreateCharacter(10);

        character.DecreaseHitPoints(10);

        Assert.Equal(0, character.HitPoints);
        Assert.Equal(0, character.TemporaryHitPoints);
        Assert.Equal(CharacterStatus.Dead, character.Status);
    }

    [Fact]
    public void DecreaseHitPoints_NonPositivePoints_NoOp()
    {
        Character character = CreateCharacter(25);

        character.DecreaseHitPoints(0);
        character.DecreaseHitPoints(-5);

        Assert.Equal(25, character.HitPoints);
    }

    [Fact]
    public void IncreaseHitPoints_BelowBase_AddsPoints()
    {
        Character character = CreateCharacter(25);
        character.DecreaseHitPoints(10); // HitPoints = 15, BaseHitPoints stays 25

        character.IncreaseHitPoints(5);

        Assert.Equal(20, character.HitPoints);
    }

    [Fact]
    public void IncreaseHitPoints_AboveBase_CapsAtBase()
    {
        Character character = CreateCharacter(25);
        character.DecreaseHitPoints(5); // HitPoints = 20

        character.IncreaseHitPoints(100);

        Assert.Equal(25, character.HitPoints);
    }

    [Fact]
    public void IncreaseHitPoints_NonPositivePoints_NoOp()
    {
        Character character = CreateCharacter(25);
        character.DecreaseHitPoints(10); // HitPoints = 15

        character.IncreaseHitPoints(0);
        character.IncreaseHitPoints(-5);

        Assert.Equal(15, character.HitPoints);
    }

    [Fact]
    public void AddTemporaryHitPoints_NonPositivePoints_NoOp()
    {
        Character character = CreateCharacter(25, temporaryHitPoints: 5);

        character.AddTemporaryHitPoints(0);
        character.AddTemporaryHitPoints(-5);

        Assert.Equal(5, character.TemporaryHitPoints);
    }

    [Fact]
    public void AddTemporaryHitPoints_PositivePoints_AddsToCurrentValue()
    {
        // Documents the current (additive) implementation. CHALLENGE.md specifies
        // temporary HP should take the higher value instead of stacking -- flagging
        // this as a known gap rather than silently asserting it's "correct."
        Character character = CreateCharacter(25, temporaryHitPoints: 5);

        character.AddTemporaryHitPoints(10);

        Assert.Equal(15, character.TemporaryHitPoints);
    }
}
