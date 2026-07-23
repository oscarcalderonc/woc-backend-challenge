import { ActionPanel } from "./components/ActionPanel/ActionPanel";
import { CharacterCard } from "./components/CharacterCard/CharacterCard";
import { useCharacter } from "./hooks/useCharacter";
import styles from "./App.module.css";

function App() {
  const { character, loading, error, refresh } = useCharacter();

  return (
    <div className={styles.page}>
      <main className={styles.main}>
        <p className="visually-hidden" role="status" aria-live="polite">
          {error
            ? `Error: ${error}`
            : character
              ? `${character.name} has ${character.hitPoints} of ${character.maxHitPoints} hit points`
              : "Loading character"}
        </p>

        {loading && !character && (
          <p className={styles.status}>Loading character…</p>
        )}

        {error && !character && (
          <p className={styles.statusError} role="alert">
            {error}
          </p>
        )}

        {character && (
          <>
            {error && (
              <p className={styles.statusError} role="alert">
                {error} — showing last known state.
              </p>
            )}
            <CharacterCard character={character} />
            <ActionPanel onAction={refresh} />
          </>
        )}
      </main>
    </div>
  );
}

export default App;
