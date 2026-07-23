using System.Text.Json.Serialization;
using System.Text.Json;

namespace woc_backend_challenge.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DamageType
{
    Bludgeoning, Piercing, Slashing, Fire, Cold, Acid,
    Thunder, Lightning, Poison, Radiant, Necrotic, Psychic, Force
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CharacterStatus
{
    Alive, Dead
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DefenseType
{
    Resistance, // assumed as reducing 50% of the damage inflicted
    Immunity    // assumed as being 0% of the damage inflicted
}

public class Character
{

    public Character()
    {
        Guid uuid = Guid.NewGuid();
        this.Id = uuid.ToString();
    }

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public CharacterStatus Status { get; set; } = CharacterStatus.Alive;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("maxHitPoints")]
    public int MaxHitPoints { get; set; }

    [JsonPropertyName("temporaryHitPoints")]
    public int TemporaryHitPoints { get; set; }

    private int _hitPoints;

    [JsonPropertyName("hitPoints")]
    public int HitPoints
    {
        get => _hitPoints;
        set
        {
            // Base HP should never be empty, so it gets initialized
            _hitPoints = value;
            if (this.MaxHitPoints == 0)
            {
                this.MaxHitPoints = value;
            }
        }
    }

    [JsonPropertyName("classes")]
    public List<CharacterClass> Classes { get; set; } = new();

    [JsonPropertyName("stats")]
    public CharacterStats Stats { get; set; } = new();

    [JsonPropertyName("items")]
    public List<CharacterItem> Items { get; set; } = new();

    [JsonPropertyName("defenses")]
    public List<CharacterDefense> Defenses { get; set; } = new();

    public CharacterDefense? GetCharacterDefense(DamageType type)
    {
        return this.Defenses.Find(x => x.Type == type);
    }

    public int GetTotalHitPoints()
    {
        return this.TemporaryHitPoints + this.MaxHitPoints;
    }

    public void IncreaseHitPoints(int points)
    {
        if (points <= 0)
        {
            return;
        }

        lock (_lock)
        {
            int newHitPoints = points + this.HitPoints;
            // We increase Hit Points up to its base Hit Points
            if (newHitPoints > this.MaxHitPoints)
            {
                newHitPoints = this.MaxHitPoints;
            }
            this.HitPoints = newHitPoints;
        }
    }

    public void DecreaseHitPoints(int points)
    {
        if (points <= 0)
        {
            return;
        }

        lock (_lock)
        {
            // If the hit gets over HP + temp HP, character is dead
            if (this.GetTotalHitPoints() <= points)
            {
                this.TemporaryHitPoints = 0;
                this.HitPoints = 0;
                this.Status = CharacterStatus.Dead;
                return;
            }

            // Otherwise, we just get the difference and decrease from temp HP first
            int remainingPoints = this.TemporaryHitPoints - points;
            switch (remainingPoints)
            {
                case <= 0:
                    this.TemporaryHitPoints = 0;
                    this.HitPoints += remainingPoints;
                    break;
                case > 0:
                    this.TemporaryHitPoints -= points;
                    break;
            }
        }
    }

    public void AddTemporaryHitPoints(int points)
    {
        if (points <= 0)
        {
            return;
        }

        lock (_lock)
        {
            this.TemporaryHitPoints += points;
        }
    }

    // Adding a lock, there are other approaches but could be overkill
    private readonly object _lock = new();

}

public class CharacterClass
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("hitDiceValue")]
    public int HitDiceValue { get; set; }

    [JsonPropertyName("classLevel")]
    public int ClassLevel { get; set; }
}

public class CharacterStats
{
    [JsonPropertyName("strength")]
    public int Strength { get; set; }

    [JsonPropertyName("dexterity")]
    public int Dexterity { get; set; }

    [JsonPropertyName("constitution")]
    public int Constitution { get; set; }

    [JsonPropertyName("intelligence")]
    public int Intelligence { get; set; }

    [JsonPropertyName("wisdom")]
    public int Wisdom { get; set; }

    [JsonPropertyName("charisma")]
    public int Charisma { get; set; }
}

public class CharacterItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("modifier")]
    public ItemModifier Modifier { get; set; } = new();
}

public class ItemModifier
{
    [JsonPropertyName("affectedObject")]
    public string AffectedObject { get; set; } = string.Empty;

    [JsonPropertyName("affectedValue")]
    public string AffectedValue { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public int Value { get; set; }
}

public class CharacterDefense
{
    [JsonPropertyName("type")]
    public DamageType Type { get; set; }

    [JsonPropertyName("defense")]
    public DefenseType Defense { get; set; }
}
