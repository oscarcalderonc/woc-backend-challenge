import { API_BASE_URL, CHARACTER_ID } from "./config";
import type { ApiErrorBody, Character, DamageType } from "./types";

export class ApiError extends Error {
  status: number;
  body: ApiErrorBody | null;

  constructor(status: number, body: ApiErrorBody | null) {
    super(body?.error ?? `Request failed with status ${status}`);
    this.name = "ApiError";
    this.status = status;
    this.body = body;
  }
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...init,
    headers: {
      "Content-Type": "application/json",
      ...init?.headers,
    },
  });

  if (!response.ok) {
    let body: ApiErrorBody | null = null;
    try {
      body = await response.json();
    } catch {
      body = null;
    }
    throw new ApiError(response.status, body);
  }

  return (await response.json()) as T;
}

export function getCharacter(characterId: string = CHARACTER_ID) {
  return request<Character>(`/api/character/${characterId}`);
}

export function dealDamage(
  hitPoints: number,
  damageType: DamageType,
  characterId: string = CHARACTER_ID,
) {
  return request<{
    message: string;
    characterId: string;
    hitPoints: number;
    damageType: string;
  }>(`/api/character/${characterId}/damage`, {
    method: "POST",
    body: JSON.stringify({ hitPoints, damageType }),
  });
}

export function healCharacter(
  healPoints: number,
  characterId: string = CHARACTER_ID,
) {
  return request<{ message: string; characterId: string; healPoints: number }>(
    `/api/character/${characterId}/heal`,
    {
      method: "POST",
      body: JSON.stringify({ healPoints }),
    },
  );
}

export function addTemporaryHitPoints(
  temporaryHitPoints: number,
  characterId: string = CHARACTER_ID,
) {
  return request<{
    message: string;
    characterId: string;
    temporaryHitPoints: number;
  }>(`/api/character/${characterId}/temp_hit_points`, {
    method: "POST",
    body: JSON.stringify({ temporaryHitPoints }),
  });
}
