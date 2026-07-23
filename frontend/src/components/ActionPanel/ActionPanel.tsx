import { DamageForm } from "./DamageForm";
import { HealForm } from "./HealForm";
import { TempHpForm } from "./TempHpForm";
import styles from "./ActionPanel.module.css";

export function ActionPanel({ onAction }: { onAction: () => void }) {
  return (
    <section className={styles.panel} aria-labelledby="actions-heading">
      <h2 id="actions-heading" className={styles.heading}>
        Actions
      </h2>
      <div className={styles.grid}>
        <DamageForm onDone={onAction} />
        <HealForm onDone={onAction} />
        <TempHpForm onDone={onAction} />
      </div>
    </section>
  );
}
