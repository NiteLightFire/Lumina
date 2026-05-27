# Lumina

**Lumina** — desktop application for task and habit management built with C# and Avalonia UI.

All data is stored locally on the user’s device (JSON-based storage).

---

## About the project

Lumina helps structure daily routines, track habits, and manage tasks through a clean MVVM-based UI.

---

## Features

- ✅ Task management (create, edit, complete, color tagging)
- 🔥 Habit tracking with streak system
- 📅 Calendar views:
  - Day view (hour-based schedule)
  - Month view
- ⚙️ Settings (UI configuration, layout options)
- 🎨 Basic customization (colors for tasks and habits)

---

## Tech stack

- C#
- Avalonia UI
- .NET 9
- MVVM architecture (CommunityToolkit.Mvvm)
- JSON local storage

---

## Project structure

```text
Lumina/
├── Lumina.sln
├── Lumina/
│   ├── Lumina.csproj
│   ├── App.axaml
│   ├── Program.cs
│   ├── Assets/
│   ├── Models/
│   ├── Services/
│   ├── ViewModels/
│   ├── Views/
│   └── ViewLocator.cs
├── README.md
└── .gitignore