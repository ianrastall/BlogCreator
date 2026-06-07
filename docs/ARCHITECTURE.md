# BlogCreator architecture

## Repository boundaries

BlogCreator is independent of both MDEdit and the website repository. MDEdit is a reference implementation only and must never become a project dependency.

## Layers

- `BlogCreator.Core` contains post models and service contracts.
- `BlogCreator.Application` contains MVVM view models and user workflows.
- `BlogCreator.Infrastructure` will contain Markdown, filesystem, Git, website-repository, media, and publication services.
- `BlogCreator.WinUI` contains WinUI 3 views, Windows integration, and dependency injection.
- `BlogCreator.Tests` validates non-UI behavior.

## Planned milestones

1. Buildable dark-mode publishing shell.
2. Local draft persistence and Astro-compatible front matter.
3. Monaco editor hosted locally in WebView2 for Markdown syntax highlighting.
4. Repository selection, Git status, commit, pull, and push.
5. Fooliosity publication workflow and deployment monitoring.
6. Media management, recovery snapshots, diagnostics, and packaging.
