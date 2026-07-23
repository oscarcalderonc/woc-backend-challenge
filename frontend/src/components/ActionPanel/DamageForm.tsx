import { useId, useState, type FormEvent } from "react";
import { dealDamage } from "../../api/client";
import { DAMAGE_TYPES, type DamageType } from "../../api/types";
import { useAsyncAction } from "../../hooks/useAsyncAction";
import styles from "./ActionPanel.module.css";

export function DamageForm({ onDone }: { onDone: () => void }) {
  const amountId = useId();
  const typeId = useId();
  const errorId = useId();
  const [amount, setAmount] = useState("");
  const [damageType, setDamageType] = useState<DamageType>(DAMAGE_TYPES[0]);

  const { run, pending, error } = useAsyncAction(
    (points: number, type: DamageType) => dealDamage(points, type),
    onDone,
  );

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    const points = Number(amount);
    if (!Number.isFinite(points) || points <= 0) return;
    const ok = await run(points, damageType);
    if (ok) setAmount("");
  }

  return (
    <form className={styles.form} onSubmit={handleSubmit}>
      <fieldset className={styles.fieldset}>
        <legend className={styles.legend}>Deal damage</legend>
        <div className={styles.row}>
          <div className={styles.field}>
            <label className={styles.label} htmlFor={amountId}>
              Amount
            </label>
            <input
              id={amountId}
              className={styles.input}
              type="number"
              min={1}
              step={1}
              inputMode="numeric"
              required
              value={amount}
              onChange={(e) => setAmount(e.target.value)}
              aria-describedby={error ? errorId : undefined}
            />
          </div>
          <div className={styles.field}>
            <label className={styles.label} htmlFor={typeId}>
              Damage type
            </label>
            <select
              id={typeId}
              className={styles.input}
              value={damageType}
              onChange={(e) => setDamageType(e.target.value as DamageType)}
            >
              {DAMAGE_TYPES.map((type) => (
                <option key={type} value={type}>
                  {type}
                </option>
              ))}
            </select>
          </div>
        </div>
        <button
          type="submit"
          className={`${styles.button} ${styles.danger}`}
          disabled={pending}
          aria-busy={pending}
        >
          Deal Damage
        </button>
        {error && (
          <p id={errorId} role="alert" className={styles.error}>
            {error}
          </p>
        )}
      </fieldset>
    </form>
  );
}
