# HP Tracker — Frontend

React + TypeScript (Vite) UI for the character HP API. It shows a character's
details and HP, and lets you deal damage, heal, and set temporary HP.

## Prerequisites

- Node.js 18+ and npm
- The backend API running (default `http://localhost:5080`). See
  [`../OSCAR-README.md`](../OSCAR-README.md) for how to start it.

## Running it

```bash
npm install     # first time only
npm run dev     # dev server at http://localhost:5173
```

## Scripts

| Script            | What it does                                |
| ----------------- | ------------------------------------------- |
| `npm run dev`     | Start the Vite dev server with HMR          |
| `npm run build`   | Type-check (`tsc -b`) and build for production |
| `npm run lint`    | Run ESLint                                  |
| `npm run preview` | Serve the production build locally          |

## Configuration

| Variable            | Default                 | Purpose                  |
| ------------------- | ----------------------- | ------------------------ |
| `VITE_API_BASE_URL` | `http://localhost:5080` | Base URL of the HP API   |

Set it in `frontend/.env.local`, for example:

```bash
VITE_API_BASE_URL=http://localhost:5080
```

The backend must allow CORS from the frontend origin
(`http://localhost:5173` in dev), otherwise the browser blocks the requests.

### Known limitation: hardcoded character id

The API assigns each character a UUID at load time and exposes no endpoint to
discover it. `src/api/config.ts` therefore hardcodes `CHARACTER_ID`. If the
backend restarts and re-seeds with a new id, update that constant or the
character will fail to load and the UI will show an error.

## Structure

```
src/
  api/          Typed API client, config, and response types
  hooks/        useCharacter (polling), useAsyncAction (pending/error state for form actions)
  components/
    CharacterCard/   Name, level/classes, status, defenses; composes the rest
    HpMeter/         HP bar with status label (Healthy/Wounded/Critical/Down)
    StatGrid/        Ability scores and modifiers
    ActionPanel/     Damage, Heal, and Temporary HP forms
```

## Behavior notes

- **Live updates:** `useCharacter` polls the API every 4 seconds and refetches
  immediately after every action.
- **Accessibility:** forms use labeled inputs, errors use `role="alert"`, the
  HP meter exposes `role="progressbar"`, and status is shown with text as well
  as color.
- **Theming:** light and dark themes follow `prefers-color-scheme`.
