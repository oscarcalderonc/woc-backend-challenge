export type CharacterStatus = "Alive" | "Dead";

export type DamageType =
  | "Bludgeoning"
  | "Piercing"
  | "Slashing"
  | "Fire"
  | "Cold"
  | "Acid"
  | "Thunder"
  | "Lightning"
  | "Poison"
  | "Radiant"
  | "Necrotic"
  | "Psychic"
  | "Force";

export const DAMAGE_TYPES: DamageType[] = [
  "Bludgeoning",
  "Piercing",
  "Slashing",
  "Fire",
  "Cold",
  "Acid",
  "Thunder",
  "Lightning",
  "Poison",
  "Radiant",
  "Necrotic",
  "Psychic",
  "Force",
];

export type DefenseType = "Resistance" | "Immunity";

export interface CharacterClass {
  name: string;
  hitDiceValue: number;
  classLevel: number;
}

export interface CharacterStats {
  strength: number;
  dexterity: number;
  constitution: number;
  intelligence: number;
  wisdom: number;
  charisma: number;
}

export interface CharacterItem {
  name: string;
  modifier?: {
    affectedObject: string;
    affectedValue: string;
    value: number;
  };
}

export interface CharacterDefense {
  type: DamageType;
  defense: DefenseType;
}

export interface Character {
  id: string;
  status: CharacterStatus;
  name: string;
  level: number;
  hitPoints: number;
  maxHitPoints: number;
  temporaryHitPoints: number;
  classes: CharacterClass[];
  stats: CharacterStats;
  items: CharacterItem[];
  defenses: CharacterDefense[];
}

export interface ApiErrorBody {
  error: string;
  [key: string]: unknown;
}
