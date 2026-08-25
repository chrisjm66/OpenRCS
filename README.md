# OpenRCS

**Open Source Rail Control Simulator**

OpenRCS is a railroad signalling simulation that models real physical infrastructure — tracks with gradients and curvature, electrical track circuits, points, and signals — rather than abstracting rail operations into simplified game rules. Safe train movement emerges from the simulated system itself, the same way it does on a real railway.

Learn more at [chrismangan.net/openrcs](https://chrismangan.net/openrcs).

## Features

- **Interlocking logic** — routes, points, and track occupancy interact to determine valid signal aspects, so safe operation is a consequence of the simulated infrastructure rather than a hard-coded rule.
- **Scenario editor** — design custom railways by laying out tracks, signals, and diagrams, then build operational scenarios around them.
- **Timetables** — run scheduled services and introduce delays, conflicts, and disruptions that need to be resolved in real time.
- **Layered modularity** — physical infrastructure is modeled separately from its diagram representation, so simulations can scale from small test layouts to complex networks, and new signalling systems can be added independently.

## Tech Stack

- [Tauri](https://tauri.app/) — desktop app shell (Rust backend, native webview)
- [SvelteKit](https://svelte.dev/) + TypeScript — frontend UI
- [Tailwind CSS](https://tailwindcss.com/) — styling
- [Specta](https://github.com/specta-rs/specta) / [tauri-specta](https://github.com/specta-rs/tauri-specta) — type-safe Rust ↔ TypeScript bindings
- [Zod](https://zod.dev/) — runtime schema validation on the frontend

The simulation engine itself lives in a dedicated Rust crate (`src-tauri/crates/simulation`), covering the track/signal/switch layout model, track circuits, diagrams, and scenarios, kept independent of the Tauri app shell.

## Project Structure

```
src/                      SvelteKit frontend
  routes/                 App screens (menu, simulation selection, simulation view)
  lib/                     Shared components, simulation state, canvas rendering
src-tauri/                Tauri app shell (Rust)
  src/                    Tauri entry point and commands
  crates/simulation/      Core simulation engine
    src/layout/           Tracks, switches, signals, track circuits
    src/diagram/          Diagram representation
    src/scenario/         Scenario definitions
```

## Getting Started

### Prerequisites

- [Node.js](https://nodejs.org/) and [pnpm](https://pnpm.io/)
- [Rust](https://www.rust-lang.org/tools/install) and the [Tauri prerequisites](https://tauri.app/start/prerequisites/) for your OS

### Install dependencies

```sh
pnpm install
```

### Run in development

```sh
pnpm tauri dev
```

### Build

```sh
pnpm tauri build
```

### Other useful scripts

```sh
pnpm check      # type-check the Svelte/TypeScript codebase
pnpm lint       # check formatting and lint rules
pnpm format     # auto-format with Prettier
```

## License

OpenRCS is licensed under the [GNU General Public License v3.0](LICENSE).
