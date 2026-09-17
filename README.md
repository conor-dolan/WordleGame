# WordleGame

A cross-platform Wordle-style game built with **.NET MAUI**.

## Features

- Player login with saved name (using local preferences)
- 5-letter word guessing gameplay
- Up to 6 attempts per round
- Guess feedback using:
  - `G` = correct letter, correct position
  - `Y` = correct letter, wrong position
  - `X` = letter not in word
- Local scoreboard saved on device
- Resettable scoreboard
- Light/Dark theme toggle in settings

## Tech Stack

- .NET 8
- .NET MAUI
- MVVM pattern
- Local JSON persistence for scores

## Project Structure

- `WordleGame/View` – XAML pages (Login, Game, Settings, Scoreboard)
- `WordleGame/ViewModel` – application logic and state
- `WordleGame/Services` – word-fetching/game data services
- `WordleGame/Model` – data models

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- .NET MAUI workloads installed

```bash
dotnet workload install maui
```
### Build

dotnet restore
dotnet build /home/runner/work/WordleGame/WordleGame/WordleGame.sln

### Run

dotnet build /home/runner/work/WordleGame/WordleGame/WordleGame/WordleGame.csproj -t:Run -f net8.0-windows10.0.19041.0


