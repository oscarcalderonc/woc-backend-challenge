import type { Character } from "../../api/types";
import { HpMeter } from "../HpMeter/HpMeter";
import { StatGrid } from "../StatGrid/StatGrid";
import styles from "./CharacterCard.module.css";

function describeClasses(character: Character): string {
  return character.classes
    .map((c) => `${c.name} ${c.classLevel}`)
    .join(" / ");
}

export function CharacterCard({ character }: { character: Character }) {
  const isDead = character.status === "Dead";

  return (
    <section className={styles.card} aria-labelledby="character-name">
      <header className={styles.header}>
        <div>
          <h1 id="character-name" className={styles.name}>
            {character.name}
          </h1>
          <p className={styles.subtitle}>
            Level {character.level} &middot; {describeClasses(character)}
          </p>
        </div>
        <span
          className={`${styles.statusPill} ${isDead ? styles.dead : styles.alive}`}
        >
          {character.status}
        </span>
      </header>

      <HpMeter
        current={character.hitPoints}
        max={character.maxHitPoints}
        temporary={character.temporaryHitPoints}
      />

      <StatGrid stats={character.stats} />

      {character.defenses.length > 0 && (
        <div className={styles.defenses}>
          {character.defenses.map((defense) => (
            <span
              key={`${defense.type}-${defense.defense}`}
              className={`${styles.defenseTag} ${
                defense.defense === "Immunity" ? styles.immunity : styles.resistance
              }`}
            >
              {defense.defense === "Immunity" ? "Immune" : "Resists"} {defense.type}
            </span>
          ))}
        </div>
      )}
    </section>
  );
}
