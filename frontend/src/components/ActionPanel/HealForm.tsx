import { useId, useState, type FormEvent } from "react";
import { healCharacter } from "../../api/client";
import { useAsyncAction } from "../../hooks/useAsyncAction";
import styles from "./ActionPanel.module.css";

export function HealForm({ onDone }: { onDone: () => void }) {
  const amountId = useId();
  const errorId = useId();
  const [amount, setAmount] = useState("");

  const { run, pending, error } = useAsyncAction(
    (points: number) => healCharacter(points),
    onDone,
  );

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    const points = Number(amount);
    if (!Number.isFinite(points) || points <= 0) return;
    const ok = await run(points);
    if (ok) setAmount("");
  }

  return (
    <form className={styles.form} onSubmit={handleSubmit}>
      <fieldset className={styles.fieldset}>
        <legend className={styles.legend}>Heal</legend>
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
        </div>
        <button
          type="submit"
          className={`${styles.button} ${styles.good}`}
          disabled={pending}
          aria-busy={pending}
        >
          Heal
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
