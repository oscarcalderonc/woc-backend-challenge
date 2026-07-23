# Session Reflection Prompt

Paste this as your last message in the AI chat at the end of each session,
before closing it. Save the AI's response verbatim as
`process/session-NN.md` (increment the number each session).

---

This chat session on the frontend challenge is ending. Using `history.md` as
your ground truth for anything earlier in this session, plus the
conversation still visible above for detail `history.md` doesn't capture —
and nothing else — write a `process.md` with these sections. Do not invent,
embellish, soften, or omit anything.

## Session metadata
- Date/time, AI tool + model name, approximate session duration, what part
  of the UI this session covered.

## Stated objective
One sentence: what did the human say they were trying to accomplish when
this session started (paraphrase their actual first message, don't invent
intent)?

## Prompt log
A chronological list of each substantive instruction/request the human gave
(not every message — collapse follow-up clarifications into the request
they belong to). One line each: what was asked, in the human's own framing.

## Output assessment
For each significant piece of code or suggestion you produced: was it
accepted as-is, modified by the human, or rejected? If modified or
rejected, what was wrong with it or what did the human change, and did they
say why?

## Errors and corrections
Any point where you produced something incorrect, hallucinated an
API/prop that doesn't exist, missed a stated constraint (TypeScript, CSS
Modules, accessibility, responsiveness), or otherwise got it wrong. State
how it was caught — did the human catch it by reading the code, by running
it, by asking a follow-up question that exposed the gap, or did you catch it
yourself?

## Human's own contribution
What did the human write, fix, or decide without asking you, this session?

## Verification performed
What did the human actually do to confirm the code works — ran it, wrote a
test, inspected in browser, or just read it and moved on? Be specific about
what you observed them doing, not what they should have done.

## Working pattern (for the reviewer)
Answer plainly, based only on evidence in this transcript:
- Did the human specify constraints (accessibility, types, responsive
  behavior) upfront, or discover them through trial and error?
- Did they ask clarifying questions before big asks, or dive straight into
  implementation requests?
- Was there a point where they redirected you away from a wrong approach?
  Quote or paraphrase it if so.
- Based on how they discussed the code (questions asked, changes
  requested), how much of the generated code do you have evidence they
  understood, versus accepted without comment? Don't guess beyond what's in
  the transcript.

## Open questions
Anything the human flagged as uncertain, deferred, or planned to revisit.
