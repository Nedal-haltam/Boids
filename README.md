# 🐦 Boids Simulation in C\#

A high-performance, interactive flocking simulation (Boids model) implemented in C# using [Raylib-cs](https://github.com/ChrisDill/Raylib-cs). This project visualizes flocking behavior of autonomous agents ("boids") based on alignment, cohesion, and separation rules.

## 📽️ Demo

The simulation starts with a welcome screen and instructions. Press `Enter` to start rendering the boids. You can dynamically adjust behavioral parameters using your keyboard.

---

## 🔧 Features

* Multithreaded update logic (custom `Update` and `Parallel.For` versions)
* Adjustable parameters for flocking:

  * `A` - Max Speed
  * `I` - Min Speed
  * `T` - Turn Factor
  * `C` - Centering Factor
  * `V` - Avoidance Factor
  * `M` - Matching Factor
* Reset parameters with `R`
* Toggle help screen with `H`
* Visual boundary box and parameter overlay

---

## 🧠 Behavior Rules

Each boid updates its position and velocity based on nearby boids:

* **Separation**: Avoid crowding neighbors
* **Alignment**: Align direction with nearby boids
* **Cohesion**: Move toward the average position of neighbors
* **Boundary avoidance**: Steer away from screen edges

---

## ⌨️ Controls

| Key         | Action                                          |
| ----------- | ----------------------------------------------- |
| `Enter`     | Start the simulation                            |
| `H`         | Show/hide the welcome/help screen               |
| `A` + `←/→` | Decrease/increase max speed                     |
| `I` + `←/→` | Decrease/increase min speed                     |
| `T` + `←/→` | Decrease/increase turn factor                   |
| `C` + `←/→` | Decrease/increase centering (cohesion) factor   |
| `V` + `←/→` | Decrease/increase avoidance (separation) factor |
| `M` + `←/→` | Decrease/increase matching (alignment) factor   |
| `R`         | Reset all parameters to default                 |

---

## 🚀 Getting Started

### 📦 Prerequisites

* [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
* [Raylib-cs](https://github.com/ChrisDill/Raylib-cs)

### 🛠️ Build & Run

```bash
git clone https://github.com/Nedal-haltam/Boids.git
cd Boids
dotnet run
```

---

## ⚙️ Customization

To modify the number of boids or simulation dimensions, edit these constants in `Program.cs`:

```csharp
static int N = 5 * 1000; // Number of boids
static int f = 100;
static int w = 9 * f;
static int h = 9 * f;
```

---

## 🧪 Performance Notes

* Uses multi-threading for efficient updates on large boid counts.

---