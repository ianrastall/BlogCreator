# Codex instructions for BlogCreator

Read [`CODEX_HANDOFF.md`](CODEX_HANDOFF.md) before making changes. It is the canonical project handoff and records the architecture, current state, build commands, constraints, and prioritized roadmap.

Non-negotiable boundaries:

- This repository is the standalone BlogCreator application.
- Do not modify, copy files into, add project references to, or otherwise couple this repository to `ianrastall/MDEdit`.
- Do not place the Fooliosity Astro website inside this repository. The website lives in `ianrastall/ianrastall.github.io`.
- Preserve C# 14, .NET 10, WinUI 3, MVVM, dark-mode-first UI, and the existing layered solution.
- Keep application logic out of code-behind except for unavoidable WinUI/WebView2 integration.
- Run restore, tests, and the WinUI build before declaring work complete.
- Make focused commits and keep the repository buildable at milestone boundaries.

The user works in VS Code and GitHub Desktop and expects complete, step-by-step commands rather than assumed framework knowledge.
