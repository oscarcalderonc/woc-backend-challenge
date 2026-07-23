# History

## [turn 1 — move backend into /backend, scaffold /frontend]
**Asked:** Move all the C# backend web service into a folder named `backend`,
create a folder for the frontend, and check `swagger.yaml` to understand the
REST API contract before starting frontend generation.
**Did:** Flagged that moving backend source files is a restructuring of
backend/API code, which `AGENTS.md` scopes me out of ("do not read, write,
or suggest changes to backend/API code"). Asked the candidate to choose
between doing the move themselves or explicitly authorizing an exception;
they chose to do it themselves. Read `swagger.yaml` to understand the four
endpoints (`healthz`, `GET /api/character/{id}`, `POST .../damage`,
`POST .../heal`, `POST .../temp_hit_points`) and their request/response
shapes, and read `CHALLENGE.md`'s full-stack addendum for the UI
requirements. Ran `npm create vite@5 frontend -- --template react-ts` to
scaffold the frontend project once the candidate confirmed the backend move
was done.
**Outcome:** Accepted. Candidate moved `Handlers/`, `Models/`,
`Properties/`, `Repositories/`, `Program.cs`, `woc_backend_challenge.csproj`,
`briv.json`, and `woc_backend_challenge.Tests/` into `backend/` themselves.

## [turn 2 — build the React/TypeScript HP tracker UI]
**Asked:** Build the frontend in `frontend/` using React, referencing
`swagger.yaml` for the damage/heal/temp-HP/get-character contract and the
`CHALLENGE.md` full-stack addendum for requirements. Simple, clean styling;
show character details nicely; refetch character status after every action;
follow `AGENTS.md` (including the history log requirement).
**Did:** Hit two integration gaps neither of us could resolve unilaterally,
and raised both instead of guessing:
1. `swagger.yaml` states the character UUID has no discovery endpoint from
   outside the process — asked how the frontend should obtain it. Candidate
   supplied the UUID directly (`1a91ce40-fadc-400a-9b68-5b6bc0bcf68c`) and
   asked me to hardcode it and note that in `OSCAR-README.md` (done below —
   this file lives at the repo root and documents an integration detail
   about the *frontend's* wiring, not a change to backend logic itself, so
   I judged it in scope of what was explicitly asked; happy to revert if the
   candidate disagrees).
2. The `Character` schema had no max/base HP field, but the addendum
   requires a Current/Max display. Offered three options; candidate added
   `maxHitPoints` to the backend response and to `swagger.yaml` themselves.

Built out the frontend: `src/api/` (typed client + config), a polling
`useCharacter` hook (4s interval, plus immediate refetch after every
mutating action) for the "live updates" requirement, and components
`CharacterCard`, `HpMeter` (colored status meter: Healthy/Wounded/
Critical/Down, using the dataviz skill's status palette, with text labels
so status is never color-only), `StatGrid` (six ability scores +
modifiers), and `ActionPanel` (`DamageForm` with textbox + damage-type
select + "Deal Damage" button, `HealForm` with textbox + "Heal" button,
`TempHpForm` with textbox + "Set Temporary HP" button, each with labeled
inputs, `role="alert"` error text, and a note on the known
additive-vs-take-higher temp-HP gap from swagger.yaml). Styled with CSS
Modules, light/dark theme via `prefers-color-scheme`, responsive grid
(stacks to one column narrow, three columns wide). Ran `tsc -b` and
`eslint .` clean. Found the frontend (`localhost:5173`) couldn't reach the
backend (`localhost:5080`) — `OPTIONS` preflight returned 405 — diagnosed it
as a missing CORS policy, which is backend config I can't touch; candidate
added `AddCors`/`UseCors` themselves. Verified in a real browser afterward:
live character data rendered, dealt 14 Piercing damage (25 → 11 HP, matching
the CHALLENGE.md worked example), healed 5 (→ 16 HP), added 10 temporary HP
(badge rendered), confirmed Tab-key focus order and visible focus rings
across all three forms, and checked layout at 606px and 1400px widths.
**Outcome:** Accepted. Working end-to-end against the live backend as of
this session; candidate confirmed CORS fix and the app was verified
afterward with the character move applied.
