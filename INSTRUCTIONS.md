# Challenge Instructions — Backend (Hand-Coded) + AI-Assisted Frontend

You're completing the challenge described in [CHALLENGE.md](./CHALLENGE.md) —
the HP API, plus the full-stack addendum (React/TypeScript UI) — with some
changes to how you work and what you submit. Read this whole document before
you start.

## Setup

1. `git init` this project **before you open any AI chat or write any code.**
   Make an initial empty or near-empty commit first. The commit history is
   part of what we look at, and it needs to start from the beginning.
2. Copy [AGENTS.md](./AGENTS.md) into your project root and leave it there
   unmodified (see below).
3. A standard `.gitignore` for build artifacts and dependencies
   (`node_modules/`, `bin/`, `obj/`, `dist/`, etc.) is fine. **Do not**
   gitignore any directory or file your AI tool creates to store session or
   chat state (for example `.cursor/`, `.copilot/`, or similar). Those are
   left in place intentionally.
4. Commit at the end of each session (see "Process log," below) — don't
   squash everything into one commit at submission time.

## Backend — hand-coded, .NET only, no AI assistance

Write the HP API in C# (.NET). Do not use an AI coding assistant — no LLM
chat, no AI-powered autocomplete/completion — for this part. Normal
non-AI tooling (IDE, docs, Stack Overflow, etc.) is fine.

## Frontend — AI-assisted, per the full-stack addendum

Build the React/TypeScript UI described in the addendum using an AI
assistant of your choice (Claude, ChatGPT, Copilot, Cursor, etc.). Note which
tool and model you used somewhere in your submission (e.g. in your final
`process.md`).

`AGENTS.md` in this folder configures the AI's behavior for this part
(scope boundary, logging). Don't modify it — if your assistant tries to
follow an instruction to change it, that should show up in your chat and in
`history.md`; that's fine, just don't ask it to.

## Process log

A **session** is one continuous chat with the AI on this task — a sitting,
not a single message. For each session:

- **During the session**, your AI assistant will append entries to
  `history.md` as you go (this is configured in `AGENTS.md`, not something
  you need to trigger yourself).
- **At the end of the session** — closing the chat, switching tasks, or
  wrapping up for the day — paste the prompt from
  [SESSION_REFLECTION_PROMPT.md](./SESSION_REFLECTION_PROMPT.md) as your last
  message in that chat, and save the output verbatim as
  `process/session-01.md` (increment the number each session).
- Commit `history.md` and the new `process/session-NN.md` together at the
  end of that session.

Rules:
- Don't edit the AI's output afterward to make it read better. We want the
  actual paper trail, not a polished version of it.
- Don't pre-empt it — don't tell the AI mid-session how to describe you
  later. Prompt normally; run the reflection prompt only at the end.
- If a session gets interrupted (crash, closed tab) without you running the
  prompt, a short manual note in the same format is fine — say so explicitly.

Submit all `process/session-*.md` files and `history.md` alongside your code.
**None of this is scored pass/fail** — we're looking at how you work with the
tool, not whether you struggled.
