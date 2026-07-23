import type { CharacterStats } from "../../api/types";
import styles from "./StatGrid.module.css";

const STAT_LABELS: { key: keyof CharacterStats; label: string }[] = [
  { key: "strength", label: "STR" },
  { key: "dexterity", label: "DEX" },
  { key: "constitution", label: "CON" },
  { key: "intelligence", label: "INT" },
  { key: "wisdom", label: "WIS" },
  { key: "charisma", label: "CHA" },
];

function modifierFor(score: number): string {
  const mod = Math.floor((score - 10) / 2);
  return mod >= 0 ? `+${mod}` : `${mod}`;
}

export function StatGrid({ stats }: { stats: CharacterStats }) {
  return (
    <dl className={styles.grid}>
      {STAT_LABELS.map(({ key, label }) => (
        <div className={styles.stat} key={key}>
          <dt className={styles.label}>{label}</dt>
          <dd className={styles.score}>{stats[key]}</dd>
          <dd className={styles.modifier}>{modifierFor(stats[key])}</dd>
        </div>
      ))}
    </dl>
  );
}
