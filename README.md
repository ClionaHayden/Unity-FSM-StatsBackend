# Unity FSM AI Prototype with Online Stats Backend

This project is a Unity-based FSM (Finite State Machine) AI prototype extended with an online statistics saving system backed by a .NET API and SQLite database. It demonstrates gameplay features like enemy AI, player stats tracking, and online stats storage for portfolio showcasing.

## Features

- **FSM AI Prototype**: Enemy AI using Finite State Machine logic.
- **Online Stats Saving**: Player stats such as enemies defeated and damage dealt are uploaded to a backend API.
- **Offline Stats Caching**: Stats are cached locally if offline and uploaded once connection is restored.
- **Session Duration Tracking**: Player session durations stored in SQLite.
- **Backend Dashboard**: View player sessions and stats through the backend.
- **Unity UI Elements**: Health bars, pause menu, options menu, and more.
- **Audio Manager**: Manage music and sound effects with volume controls.
- **Leaderboard**: Displays online leaderboard fetched from backend API.
- **Pause & Options Menu**: Includes volume sliders, fullscreen toggle, and backend URL input.

## Technologies

- Unity (C#)
- .NET 7 Web API with ASP.NET Core
- Entity Framework Core with SQLite for data storage
- TMPro for UI text
- Unity UI system

## Setup Instructions

### Unity Project

1. Clone this repo.
2. Open the Unity project folder.
3. Open Unity Editor (recommended Unity version included).
4. Set the backend URL in Options menu or modify PlayerPrefs key `BackendURL`.
5. Run the game and test gameplay & stats upload.

### Backend API

1. Navigate to the `StatsAPI` folder.
2. Run `dotnet restore` to install dependencies.
3. Run `dotnet ef database update` to apply migrations and create SQLite DB.
4. Run `dotnet run` to start the API server.
5. Access Swagger UI at `http://localhost:<port>/swagger` to test endpoints.

## Usage

- Play the Unity game, defeat enemies, and stats will upload automatically.
- If offline, stats are cached locally and uploaded once connection is restored.
- Use the leaderboard menu to view aggregated player stats.
- Customize audio and display settings via Options menu.
- Pause menu accessible with Escape key.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
