import { useCallback, useState } from "react";
import { ApiError } from "../api/client";

export function useAsyncAction<Args extends unknown[]>(
  action: (...args: Args) => Promise<unknown>,
  onSuccess?: () => void,
) {
  const [pending, setPending] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const run = useCallback(
    async (...args: Args) => {
      setPending(true);
      setError(null);
      try {
        await action(...args);
        onSuccess?.();
        return true;
      } catch (err) {
        setError(
          err instanceof ApiError
            ? err.message.replace(/_/g, " ")
            : "Something went wrong reaching the API.",
        );
        return false;
      } finally {
        setPending(false);
      }
    },
    [action, onSuccess],
  );

  return { run, pending, error, setError };
}
