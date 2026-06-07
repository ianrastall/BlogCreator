# BlogCreator — Codex 5.5 Handoff

## 1. Purpose

BlogCreator is a purpose-built Windows desktop application for writing, previewing, organizing, and publishing Ian Rastall's **Fooliosity** blog.

The intended daily workflow is:

1. Open BlogCreator.
2. Create or select a post.
3. Edit Markdown with syntax highlighting.
4. Fill in title, description, slug, category, tags, dates, and images through ordinary GUI fields.
5. Preview the post using styling close to the published website.
6. Save as a local draft or publish it.
7. BlogCreator writes an Astro-compatible Markdown file into a local clone of `ianrastall/ianrastall.github.io`.
8. BlogCreator safely pulls, commits, and pushes through Git.
9. GitHub Actions builds and deploys the public site.

The user should not need to learn Astro, Node.js, YAML front matter, Git commands, or GitHub Actions to operate the finished application.

## 2. Repository boundaries

These boundaries are non-negotiable.

| Repository | Responsibility |
|---|---|
| `ianrastall/BlogCreator` | This standalone WinUI 3 desktop publishing application |
| `ianrastall/ianrastall.github.io` | Fooliosity website source and GitHub Pages deployment |
| `ianrastall/MDEdit` | Independent general-purpose Markdown editor |

MDEdit was used only as an architectural reference. Do not:

- modify MDEdit as part of BlogCreator work;
- add a project reference or package dependency on MDEdit;
- copy future changes automatically from MDEdit;
- turn BlogCreator into a mode inside MDEdit;
- place Astro or website source files in BlogCreator.

Reimplement or adapt useful ideas cleanly within BlogCreator.

## 3. User and working assumptions

The user works primarily with:

- Windows;
- VS Code;
- GitHub Desktop for cloning, pulling, and basic repository operations;
- PowerShell;
- complete C# files supplied by an AI coding agent.

The user does **not** intend to learn the underlying frameworks. Therefore:

- Do not end a task with vague instructions such as “configure WinUI” or “set up Astro.”
- Supply exact file changes and exact commands.
- Prefer scripts and one-click workflows over recurring manual commands.
- Error messages should be actionable and include a copyable diagnostic report.
- Avoid requiring Visual Studio for ordinary work when a CLI build is possible.
- Keep local setup instructions short and sequential.

## 4. Current repository state

The initial bootstrap milestone is complete and merged into `main`.

Known-good capabilities:

- C# 14 configuration through `Directory.Build.props`.
- .NET 10 projects.
- WinUI 3 through the Windows App SDK.
- `CommunityToolkit.Mvvm` for MVVM source generators and commands.
- Microsoft dependency injection and generic host.
- Dark-mode three-pane shell.
- In-memory post list.
- Post metadata view model.
- Markdown editing field.
- Markdig Markdown-to-HTML rendering.
- WebView2 preview pane.
- xUnit test project.
- GitHub Actions workflow on Windows.
- Successful restore, library builds, tests, and WinUI build in CI.

The bootstrap was merged as commit:

```text
ae54513340fc8f53989543d3568e0c9a862f742b
```

Subsequent documentation commits may exist after that commit.

## 5. What is not implemented yet

The current Save Draft and Publish commands are placeholders. The application does not yet:

- persist drafts to disk;
- discover existing posts;
- serialize or parse YAML front matter;
- write to the website repository;
- manage images;
- invoke Git;
- pull or push changes;
- monitor GitHub Pages deployment;
- host Monaco Editor;
- provide real Markdown syntax highlighting in the editor;
- recover autosave snapshots;
- package or install itself.

Do not describe these as working until they are implemented and tested.

## 6. Solution structure

```text
BlogCreator.slnx
Directory.Build.props
src/
  BlogCreator.Core/
  BlogCreator.Application/
  BlogCreator.Infrastructure/
  BlogCreator.WinUI/
tests/
  BlogCreator.Tests/
docs/
  ARCHITECTURE.md
.github/
  workflows/
    build.yml
```

### `BlogCreator.Core`

Contains framework-independent domain models and service contracts.

Current notable types:

- `Models/PostDocument.cs`
- `Models/PostStatus.cs`
- `Interfaces/IMarkdownRenderer.cs`
- `Interfaces/IPostRepository.cs`
- `Interfaces/IPublishingService.cs`

Core must not reference WinUI, WebView2, Markdig, Git libraries, or filesystem-specific UI services.

### `BlogCreator.Application`

Contains MVVM view models and application workflows.

Current notable types:

- `ViewModels/PostEditorViewModel.cs`
- `ViewModels/ShellViewModel.cs`

Use `CommunityToolkit.Mvvm` attributes and generated commands/properties where appropriate. Keep workflows testable without WinUI.

### `BlogCreator.Infrastructure`

Contains implementations involving external technology:

- Markdig;
- filesystem access;
- YAML/front matter serialization;
- website repository discovery;
- Git process execution;
- media copying and optimization;
- settings persistence;
- backup and recovery snapshots;
- deployment-status queries.

Current implementation:

- `Services/MarkdownRenderer.cs`

### `BlogCreator.WinUI`

Contains:

- XAML views;
- application startup;
- dependency registration;
- Windows dialogs and pickers;
- WebView2 integration;
- future Monaco host integration.

Current notable files:

- `App.xaml`
- `App.xaml.cs`
- `Views/ShellPage.xaml`
- `Views/ShellPage.xaml.cs`

Keep code-behind limited to view-specific concerns that are impractical to express through MVVM, especially WebView2 lifecycle and editor message bridging.

### `BlogCreator.Tests`

Tests framework-independent behavior. Expand this project aggressively as persistence and publishing are implemented.

## 7. Current technology versions

At the time of this handoff, the repository uses:

```text
Target framework: net10.0
WinUI target: net10.0-windows10.0.19041.0
Architecture: x64
CommunityToolkit.Mvvm: 8.4.2
Microsoft.WindowsAppSDK: 2.1.3
Microsoft.Web.WebView2: 1.0.3967.48
Markdig: 1.2.0
Microsoft.Extensions.*: 10.0.8
xUnit: 2.9.3
```

Do not upgrade packages opportunistically while implementing unrelated features. Package upgrades should be isolated changes with CI verification.

## 8. Build and validation

Run from the repository root in PowerShell.

### Restore

```powershell
dotnet restore BlogCreator.slnx
```

### Build core projects

```powershell
dotnet build src/BlogCreator.Core/BlogCreator.Core.csproj -c Debug --no-restore
dotnet build src/BlogCreator.Application/BlogCreator.Application.csproj -c Debug --no-restore
dotnet build src/BlogCreator.Infrastructure/BlogCreator.Infrastructure.csproj -c Debug --no-restore
```

### Run tests

```powershell
dotnet test tests/BlogCreator.Tests/BlogCreator.Tests.csproj -c Debug --no-restore
```

### Build WinUI app

```powershell
dotnet build src/BlogCreator.WinUI/BlogCreator.WinUI.csproj -c Debug --no-restore -p:Platform=x64
```

### Run

```powershell
.\src\BlogCreator.WinUI\bin\Debug\net10.0-windows10.0.19041.0\win-x64\BlogCreator.WinUI.exe
```

### Required completion check

Before declaring a milestone complete:

1. Restore succeeds.
2. All non-UI projects build.
3. Tests pass.
4. WinUI app builds for x64.
5. Any new behavior has tests where practical.
6. GitHub Actions is green.

## 9. CI notes

Workflow:

```text
.github/workflows/build.yml
```

The workflow:

- runs on a Windows runner;
- installs .NET 10;
- restores the solution;
- builds core projects separately;
- runs tests;
- builds the WinUI project;
- captures `winui-build.log` as an artifact even on failure.

This artifact was deliberately added because WinUI/XAML errors can be hidden by long CI output. Preserve this diagnostic behavior.

## 10. Domain model direction

`PostDocument` is currently minimal. Evolve it carefully toward the website contract.

Expected post properties include:

```text
Id
Title
Description
Slug
Category or Categories
Tags
Body
Status
PublishedAt
UpdatedAt
HeroImage
Draft
Featured
CanonicalUrl, if needed later
```

Avoid maintaining two conflicting post representations. Establish one canonical domain model and map it to:

- editable view-model properties;
- YAML front matter;
- local drafts;
- published website files.

## 11. Proposed website post contract

Coordinate this with `ianrastall/ianrastall.github.io/CODEX_HANDOFF.md` before finalizing it.

A likely Markdown file should look like:

```markdown
---
title: "Example title"
description: "Concise summary for listings and metadata."
published: 2026-06-07T18:30:00-05:00
updated: 2026-06-07T18:30:00-05:00
slug: "example-title"
category: "Film"
tags:
  - Blade Runner
  - Film analysis
draft: false
featured: false
heroImage: "/images/posts/example-title/hero.webp"
---

# Optional visible heading

Post body.
```

The exact schema must be defined once in both repositories and tested. Do not silently diverge.

## 12. Next milestone: local persistence and front matter

This is the highest-priority implementation task.

### Required outcome

The application must be able to:

1. Choose or configure a workspace directory.
2. Load existing Markdown post files from that workspace.
3. Parse front matter into `PostDocument`.
4. Create a new post.
5. Save it atomically as Markdown plus front matter.
6. Reopen it without metadata or body loss.
7. Distinguish draft and published status.
8. Report validation errors clearly.

### Recommended implementation pieces

Core contracts:

```text
IPostSerializer
IPostRepository
IWorkspaceService
ISettingsService
```

Infrastructure implementations:

```text
AstroPostSerializer
FileSystemPostRepository
JsonSettingsService
AtomicFileWriter
```

Suggested storage strategy:

- Application settings under `%LOCALAPPDATA%\BlogCreator\`.
- Website posts stored in the selected website clone, eventually under `src/content/blog/`.
- Before replacing a post file, write a temporary file and atomically replace the original.
- Preserve UTF-8 without a BOM unless the website tooling requires otherwise.
- Use LF or a single clearly documented line-ending policy for generated Markdown.

### Required tests

At minimum:

- front matter round trip;
- quoted title handling;
- tags and categories;
- multiline body preservation;
- empty optional fields;
- invalid front matter error;
- filename/slug generation;
- atomic overwrite behavior where testable;
- loading multiple posts;
- draft/published mapping.

## 13. Monaco Editor milestone

The plain WinUI `TextBox` is a bootstrap editor only.

The final editor should be Monaco Editor hosted locally in WebView2, providing:

- Markdown syntax highlighting;
- dark theme;
- line numbers;
- find and replace;
- familiar VS Code key behavior;
- code-fence language highlighting;
- undo/redo;
- reliable transfer of text between C# and JavaScript;
- no network dependency at runtime.

Requirements:

- Bundle Monaco assets with the application or installer.
- Do not load the editor from a public CDN.
- Create a narrow message protocol between C# and the editor.
- Debounce content-change events.
- Prevent recursive C# → editor → C# update loops.
- Keep the canonical unsaved text state explicit.
- Add recovery snapshots before large or destructive transformations.

Do not attempt Monaco until persistence and the post model are stable.

## 14. Git and publishing milestone

Prefer invoking the installed Git CLI rather than embedding a full Git implementation.

Rationale:

- Git for Windows and Git Credential Manager can handle authentication.
- CLI output is familiar and easily included in diagnostics.
- It avoids storing GitHub personal access tokens inside BlogCreator.

Expected safe publication sequence:

1. Validate post and assets.
2. Save all files atomically.
3. Confirm repository status.
4. Refuse to overwrite unrelated uncommitted changes without explicit handling.
5. Fetch/pull safely.
6. Detect and report conflicts.
7. Stage only intended post and asset files where possible.
8. Commit with a clear message.
9. Push.
10. Report the commit and expected public URL.

Never run destructive commands such as `git reset --hard`, `git clean -fd`, or forced pushes as an automatic recovery strategy.

Capture:

- executable path;
- working directory;
- arguments;
- exit code;
- standard output;
- standard error;
- affected files;
- timestamp.

Expose a **Copy Diagnostic Report** action.

## 15. UI direction

The finished application should be a publishing tool, not an IDE.

Primary areas:

```text
Posts
Drafts
Published
Scheduled
Media
Categories
Tags
Site Settings
Publish Log
Diagnostics
```

Initial editor layout:

- left: posts and filters;
- center: metadata and Markdown editor;
- right: rendered preview;
- top: New, Save Draft, Preview, Publish;
- bottom: clear status and error reporting.

Requirements:

- Dark mode by default.
- Native WinUI controls for shell and forms.
- Keyboard-accessible controls.
- Clear destructive-action confirmations.
- Distinguish saving from publishing visually and semantically.
- Never claim a post was published merely because it was saved locally.

## 16. Autosave, backup, and recovery

Blog writing must be protected from data loss.

Eventually implement:

- debounced autosave;
- timestamped recovery snapshots;
- backup before publication;
- recovery prompt after an unclean shutdown;
- publication log;
- Git history as an additional recovery layer;
- explicit warnings before deleting or unpublishing.

Autosave failures must be visible; do not silently discard them.

## 17. Error-handling standard

Avoid generic messages such as:

```text
Something went wrong.
```

Provide:

- operation attempted;
- affected path or post;
- stage completed;
- stage failed;
- exception or process output;
- safe next action;
- button to copy details.

Example:

```text
Publication failed during Git push.

Repository: D:\GitHub\ianrastall.github.io
Post: src\content\blog\example-title.md

Completed:
✓ Post validated
✓ File saved
✓ Commit created

Failed:
✗ Push to origin/main

Exit code: 1
[standard error]
```

## 18. Coding standards

- Nullable reference types remain enabled.
- Use async APIs for filesystem/process operations where appropriate.
- Accept `CancellationToken` in long-running service contracts.
- Prefer immutable or narrowly mutable domain data where practical.
- Keep UI dispatching at the WinUI boundary.
- Do not block the UI thread on Git, filesystem, Markdown parsing, image conversion, or network operations.
- Use dependency injection for services.
- Keep generated Markdown deterministic.
- Do not add an AI dependency to runtime behavior.
- Avoid broad catch blocks that suppress errors.
- Do not log secrets or authentication tokens.

## 19. Source-control workflow for Codex

For substantial work:

1. Pull current `main`.
2. Create a focused branch.
3. Inspect `AGENTS.md`, this file, `README.md`, and `docs/ARCHITECTURE.md`.
4. Make the smallest coherent milestone.
5. Add or update tests.
6. Run all validation commands.
7. Update documentation when architecture or commands change.
8. Commit with a descriptive message.
9. Push and open a pull request.
10. Do not merge a failing branch.

Avoid mixing dependency upgrades, large formatting changes, and functional work in one pull request.

## 20. Definition of done for the next milestone

Local persistence/front matter is complete only when:

- the workspace location can be configured;
- existing post files load into the GUI;
- new drafts save to disk;
- metadata round-trips accurately;
- Markdown bodies round-trip byte-for-byte apart from documented newline normalization;
- failed saves do not corrupt the previous file;
- invalid posts show actionable errors;
- tests cover serializer and repository behavior;
- the WinUI project builds;
- CI passes;
- README and this handoff reflect the new behavior.

## 21. Immediate starting point for a fresh Codex session

Use this task framing:

> Implement BlogCreator milestone 2: local draft persistence and an Astro-compatible Markdown/front-matter serializer. Preserve the existing layered architecture, keep MDEdit untouched, add thorough round-trip tests, and leave Git publishing and Monaco Editor for later. Run the complete build/test sequence and update the documentation.

Before coding, inspect the corresponding website handoff in `ianrastall/ianrastall.github.io` so the proposed front matter contract remains compatible.
