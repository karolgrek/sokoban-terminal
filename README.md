## Overview

A modern, terminal-based implementation of the classic **Sokoban** puzzle game, featuring a custom **Multi-Agent** engine. 
Supports complex autonomous entities, predictive collision detection, and is built with a highly decoupled architecture.

---

## Languages and Tools

* **C# / .NET 8.0+**
* **xUnit** (for fast, headless unit and integration testing)
* **GitHub Actions** (for CI/CD automation)

---

## Project Structure

| Directory / File               | Description                                                                           |
| ------------------------------ | ------------------------------------------------------------------------------------- |
| `Program.cs`                   | Core entry point initializing the game with system I/O.                               |
| `Core/GameEngine.cs`           | Heart of the game – state management, collision detection, and two-phase tick engine. |
| `Models/GameObjects.cs`        | Domain logic and Actor Model representations (`Player`, `Crate`, `Wall`, `Agent`).    |
| `UI/ConsoleMenu.cs`            | Terminal interface, level selection, and map definitions.                             |
| `UI/IConsole.cs`               | Abstraction for I/O operations enabling Dependency Injection.                         |
| `Sokoban.Tests/`               | Separate xUnit test project for domain logic, edge-cases, and smoke testing.          |

---

## Behavior

The game engine operates on a strictly deterministic **Actor Model** pattern. 

* **Single-Agent Mode:** Classic Sokoban logic. You must push all crates onto designated targets.
* **Multi-Agent Mode:** Features autonomous agents pacing across the map. They can kill the player on impact, but can also push crates for you.
* **Predictive Collision:** Uses a look-ahead `HashSet` algorithm to predict overlapping future states in `O(N)` time, ending the game if agents collide.
* **Phased Physics:** Crates are updated in phase 1, Agents in phase 2, preventing race conditions and non-deterministic movement.

---

## How to Run

1. **Run the game**
   
   ```bash
   dotnet run
   ```
   
2. **Run the test suite** (Headless execution)

   ```bash
   dotnet test
   ```

---

## Screenshots

<p align="center">Main Menu & Level Selection</p>
<p align="center">
  <img src="https://i.imgur.com/BbfMIWJ.png" width="80%" alt="Main Menu"/>
</p>

<p align="center">Choosing difficulty</p>
<p align="center">
  <img src="https://i.imgur.com/9qzwYKv.png" width="80%" alt="Choosing Difficulty"/>
</p>

<p align="center">Single-Agent Gameplay | Multi-Agent Gameplay</p>
<p align="center">
  <img src="https://i.imgur.com/JN2Ww5Q.png" width="45%" alt="Single Agent"/>
  <img src="https://i.imgur.com/dw4mMoH.png" width="45%" alt="Multi Agent"/>
</p>

<p align="center">Tutorial</p>
<p align="center">
  <img src="https://i.imgur.com/dw4mMoH.png" width="80%" alt="Tutorial"/>
</p>

---

## Architecture & Testing

The project heavily utilizes **Dependency Injection (DI)**. By abstracting standard terminal commands (`Console.WriteLine`, `Console.ReadKey`) behind the `IConsole` interface, the domain logic is entirely decoupled from the OS terminal.

This allows the `Sokoban.Tests` project to inject a `MockConsole` during runtime. The xUnit test suite plays through entire levels instantly in memory, validating game state, complex collision edge-cases, and menu navigation without requiring a real terminal window.
