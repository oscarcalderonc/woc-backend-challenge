export const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5080";

// The API assigns each character a UUID at load time and, per swagger.yaml,
// currently exposes no endpoint to discover it from outside the process.
// Hardcoded here for execution purposes only, pointing at the id assigned
// to the briv.json seed data on this machine. See OSCAR-README.md.
export const CHARACTER_ID = "1a91ce40-fadc-400a-9b68-5b6bc0bcf68c";
