import styles from "./HpMeter.module.css";

interface HpMeterProps {
  current: number;
  max: number;
  temporary: number;
}

type Status = "good" | "warning" | "critical";

function statusFor(ratio: number): { status: Status; label: string } {
  if (ratio <= 0) return { status: "critical", label: "Down" };
  if (ratio < 0.25) return { status: "critical", label: "Critical" };
  if (ratio < 0.5) return { status: "warning", label: "Wounded" };
  return { status: "good", label: "Healthy" };
}

export function HpMeter({ current, max, temporary }: HpMeterProps) {
  const safeMax = max > 0 ? max : 1;
  const ratio = Math.max(0, Math.min(1, current / safeMax));
  const { status, label } = statusFor(ratio);

  return (
    <div className={styles.wrapper}>
      <div className={styles.headerRow}>
        <span className={`${styles.statusBadge} ${styles[status]}`}>
          <span aria-hidden="true" className={styles.statusDot} />
          {label}
        </span>
        <span className={styles.value}>
          {current} / {max} HP
        </span>
      </div>
      <div
        className={styles.track}
        role="progressbar"
        aria-valuenow={current}
        aria-valuemin={0}
        aria-valuemax={max}
        aria-label={`Hit points: ${current} of ${max}, ${label}`}
      >
        <div
          className={`${styles.fill} ${styles[status]}`}
          style={{ width: `${ratio * 100}%` }}
        />
      </div>
      {temporary > 0 && (
        <div className={styles.tempRow}>
          <span className={styles.tempBadge}>
            <span aria-hidden="true">+</span>
            {temporary} Temporary HP
          </span>
        </div>
      )}
    </div>
  );
}
