import { useCallback, useEffect, useRef, useState } from "react";
import { ApiError, getCharacter } from "../api/client";
import type { Character } from "../api/types";

const POLL_INTERVAL_MS = 4000;

interface UseCharacterResult {
  character: Character | null;
  loading: boolean;
  error: string | null;
  refresh: () => Promise<void>;
}

export function useCharacter(): UseCharacterResult {
  const [character, setCharacter] = useState<Character | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const inFlight = useRef(false);

  const refresh = useCallback(async () => {
    if (inFlight.current) return;
    inFlight.current = true;
    try {
      const data = await getCharacter();
      setCharacter(data);
      setError(null);
    } catch (err) {
      setError(
        err instanceof ApiError
          ? err.message
          : "Couldn't reach the character API. Is the backend running?",
      );
    } finally {
      inFlight.current = false;
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    refresh();
    const id = window.setInterval(refresh, POLL_INTERVAL_MS);
    return () => window.clearInterval(id);
  }, [refresh]);

  return { character, loading, error, refresh };
}
