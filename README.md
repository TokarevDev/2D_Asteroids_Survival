# 2D Asteroids Survival

An endless 2D Asteroids-style survival game built with Unity `2022.3.9f1` and C#.

The project is a graduation work and gameplay-programming portfolio sample focused on explicit architecture, custom physics, data-driven configuration, pooled runtime objects, desktop/mobile controls, platform adapters, and lifecycle-safe Unity code.

Status: feature-complete; the 21-point architecture review is closed; final Android device and release-build validation remain.

Portfolio: https://tokarevdev.github.io/

## Gameplay

The player pilots a ship inside a responsive toroidal arena and earns as many points as possible while avoiding asteroids, fragments, and pursuing UFOs.

- Movement uses acceleration, braking, inertia, rotation, and wrap-around world bounds.
- Large asteroids split into two faster fragments when destroyed by a bullet.
- Bullets destroy fragments and UFOs.
- The laser destroys every intersected enemy, has limited charges, and recharges over time.
- Asteroids spawn outside the visible world and travel with randomized velocity.
- UFOs spawn periodically, pursue the player, and select a random blue, orange, or red visual on every spawn.
- The player has three health points. Enemy contact causes damage and an elastic bounce.
- After a hit, the player receives three seconds of invulnerability, loses control temporarily, passes through enemies, and displays collision and invulnerability feedback.
- Large asteroids rotate smoothly, fragments use frame animation, collision sparks are pooled, and thruster particles react to thrust and current speed.
- The HUD shows health, score, survival time, position, rotation angle, speed, laser charges, and recharge time.

The base world configuration is `32 x 18` Unity units. Runtime camera bounds synchronize the toroidal playfield with the current aspect ratio, so the arena remains responsive across supported landscape resolutions.

## Controls

### Keyboard and mouse

| Action | Input |
| --- | --- |
| Thrust | `W` or Up Arrow |
| Brake | `S` or Down Arrow |
| Turn left/right | `A` / `D` or Left / Right Arrow |
| Fire bullets | Hold `Space` or Left Mouse Button |
| Fire laser | `E` or Right Mouse Button |

### Mobile

- A virtual joystick controls movement direction.
- Dedicated touch buttons fire bullets and the laser.
- Mobile controls are shown only on mobile platforms.
- Desktop and mobile input are separate `IPlayerInputStrategy` implementations.

The project intentionally uses Unity's classic `Input` API. The New Input System is not used.

## Architecture

Project code is divided into four high-level assembly definitions:

| Assembly | Responsibility |
| --- | --- |
| `Game.Core` | Pure models, contracts, configuration DTOs, custom-physics primitives, input contracts, navigation facade, and factories |
| `Game.Infrastructure` | JSON loading, classic input strategies, scene loading, bootstrap, Firebase, AdMob, application services, and project-level bindings |
| `Game.Gameplay` | Player, enemies, weapons, collision orchestration, pooling, score, session, world synchronization, signals, and gameplay views |
| `Game.UI` | Main menu, HUD, mobile controls, game-over flow, Views, and ViewModels |

`ProjectContext`, scene installers, and UI installers are composition roots. Zenject constructs services and exposes scene components through explicit bindings; runtime gameplay code does not search the scene with `FindObjectOfType`, `GameObject.Find`, or tag lookups.

Gameplay bindings are split by domain across `GameWorldInstaller`, `PlayerInstaller`, `EnemyLifecycleInstaller`, `EnemySpawningInstaller`, `ProjectileLifecycleInstaller`, `LaserWeaponInstaller`, and `GameSessionInstaller`. The remaining `GameInstaller` contains only shared gameplay services and fixed-loop composition. Installers describe dependencies without resolving services manually or hiding runtime strategy selection inside factory callbacks.

Pure gameplay state and algorithms are regular C# classes. `MonoBehaviour` components are limited to Unity-facing views, scene references, pools, and lifecycle adapters. MVVM is used only for UI.

`GameplayFixedLoop` is the single Zenject `IFixedTickable` entry point. It executes named stages in an explicit order: player state, movement, physics integration, projectile lifecycle, world boundaries, collisions, and presentation synchronization. Gameplay ordering is therefore visible in code instead of being encoded through unrelated numeric execution priorities.

### Custom physics

Gameplay movement and collision do not use `Physics`, `Physics2D`, dynamic `Rigidbody`, or `Rigidbody2D` simulation.

The custom physics stack contains:

- `CustomPhysicsBody2D` for position, velocity, radius, and mass;
- `CustomPhysicsIntegrator2D` and `CustomPhysicsWorld2D` for fixed-step integration;
- circle-circle and segment-circle intersection tests;
- elastic player/enemy collision resolution;
- registries and synchronizers that separate logical bodies from Unity views;
- `ToroidalWorld2D` for wrap-around coordinates.

Enemy-enemy collision is intentionally disabled. Collision queries operate on centralized registries without allocating temporary collections in hot loops.

### Data-driven configuration

Runtime balance is loaded from JSON in `Assets/StreamingAssets/Configs/` through Newtonsoft.Json:

- `player.json` — health, acceleration, braking, speed, turn rate, bullet parameters, laser parameters, and invulnerability duration;
- `enemy.json` — a stable enemy-type key-to-parameters map plus spawn timing and fragmentation settings;
- `world.json` — world size, maximum enemies, spawn offset, and initial pool sizes.

`GameConfigLoader` loads the three files during bootstrap, and `GameConfigValidator` rejects invalid values before gameplay scenes open. Enemy keys must exactly match the `EnemyType` values `LargeAsteroid`, `Fragment`, and `Ufo`; missing, unknown, null, duplicate, or incorrectly cased keys fail at the configuration boundary. JSON deserialization also rejects duplicate object properties instead of silently accepting the last value.

ScriptableObjects remain only where Unity asset references are appropriate, such as asteroid animation variants and advertising identifiers. Enemy parameter and reward lookup are built from the validated map instead of a fixed property set plus `switch`, so extending those data paths does not require modifying their consumers.

## Design patterns

### Object Pool

The non-`MonoBehaviour` generic `ObjectPool<T>` owns all created instances and reusable entries. It uses `List<T>`, `Queue<T>`, and `HashSet<T>` to expose created objects, provide FIFO reuse, and reject duplicate returns. Asteroid, UFO, projectile, enemy-entity, and collision-VFX lifecycles build on pooled storage to avoid repeated runtime `Instantiate`/`Destroy` churn.

`EnemyPool<TEnemy, TInitialization>` centralizes the complete view lifecycle: rent, type-specific initialization, entity/view registration, activation, and rollback on failure. Concrete asteroid and UFO pools provide only creation details, initialization data, and narrow visual hooks. Enemy death captures the required event data before the object is reset and returned, then publishes through the shared death event source.

### Facade

`GameNavigationFacade` provides the UI with a small navigation API for start, restart, and main-menu transitions. It centralizes the in-progress guard and prevents double-click scene requests without exposing scene-loader details to Views or ViewModels.

### Strategy

`IPlayerInputStrategy` separates `KeyboardMouseInputStrategy` from `MobileInputStrategy`. `PlayerInputStrategySelector` receives both strategies through constructor injection and selects the platform implementation. `PlayerInputStateProvider` polls the selected strategy once per rendered frame and exposes the same immutable `PlayerInputState` snapshot to movement and weapon systems.

### Factory

`EnemyEntityFactory` and `ProjectileEntityFactory` create logical runtime entities with their required state. Initial creation is separated from pooling and lifecycle orchestration, keeping construction rules out of spawners and collision systems.

### Observer

C# events and Zenject SignalBus propagate health, score, enemy-death, and player-death changes. `PlayerDeathSignalService` observes `PlayerHealth.Died` and fires `PlayerDiedSignal`; session logic, analytics, and UI can react independently with symmetric `IInitializable`/`IDisposable` subscriptions.

## Major systems

### Enemies and rewards

- Asteroids and UFOs share centralized logical enemy storage and lifecycle services.
- `EnemySpawnProcess` owns the shared timer, maximum-enemy check, and spawn flow; `AsteroidSpawnAction` and `UfoSpawnAction` own only type-specific spawn behavior.
- `EnemyPool<TEnemy, TInitialization>` provides the common transactional pooled-view lifecycle, while type-specific pools retain only creation and presentation details.
- `EnemyHealth` owns reusable health/death state; asteroid and UFO components delegate visual selection and presentation to dedicated visual collaborators.
- `EnemyLifecycleService` registers the entity, custom-physics body, `IDamageable`, and view as one operation and rolls completed stages back if a later stage fails.
- Projectile impacts and `EnemyDestructionService` resolve targets through the shared `IDamageable` contract and `EnemyDamageableRegistry`, so neither depends on concrete asteroid or UFO pools.
- Large asteroid variants are selected without immediate repetition.
- UFO visuals are randomized on each pooled spawn.
- `UfoPursuitMovement` follows the player without coupling the logical entity to a Unity component.
- `EnemyRewardService` builds `Dictionary<EnemyType, int>` from the validated enemy-parameter map and grants score only for player-caused deaths.

### Projectiles and laser

- Bullets use custom bodies, pooled views, a centralized registry, lifetime control, and world-exit cleanup.
- Projectile entity-to-view ownership uses dictionaries with symmetric registration, removal, and spawn rollback; impact handling does not scan concrete pools.
- Bullet impacts apply damage through `IDamageable`; a lethal hit on a large asteroid additionally spawns configured fragments.
- `LaserTargetQuery` performs segment-circle tests and returns every intersected enemy.
- `LaserChargeMagazine` and `LaserRechargeController` own charge consumption and recovery.
- A short-lived `LineRenderer` view presents each laser shot.

### UI and navigation

- HUD ViewModels observe gameplay state and expose presentation-ready values.
- `GamePauseService` owns time-scale changes; `GameSession` owns session completion.
- `GameOverViewModel` captures the final score and delegates navigation to `GameNavigationFacade`.
- Main-menu quit behavior is isolated behind `IApplicationQuitService`.
- Scene transitions use UniTask; gameplay coroutines and `Task` are not used.

## Firebase Analytics

Firebase Unity SDK `13.15.0` is initialized during bootstrap after JSON configuration has loaded. Dependency checking uses `CheckAndFixDependenciesAsync`, is awaited through UniTask, and fails softly except for external cancellation.

The analytics adapter implements `IAnalyticsService` and reports:

| Event | Parameters |
| --- | --- |
| `game_started` | none |
| `game_ended` | `score`, `duration_seconds` |

Android package name: `com.tokarevdev.asteroidssurvival`.

To validate events with Firebase DebugView on a connected Android device:

```bash
adb shell setprop debug.firebase.analytics.app com.tokarevdev.asteroidssurvival
```

Play a session, end the game, and verify `game_started` and `game_ended` in Firebase DebugView. Disable debug mode afterward:

```bash
adb shell setprop debug.firebase.analytics.app .none.
```

The Editor warning `Database URL not set in the Firebase config` is expected: this project uses Firebase Analytics and does not use Firebase Realtime Database.

Firebase native binaries are tracked through Git LFS.

## Advertising

Google Mobile Ads `11.2.0` is isolated behind `IAdvertisementService`.

- `AdMobInitializer` owns SDK initialization.
- `BannerAdvertisementService` owns banner creation, display, callbacks, and destruction.
- `AdMobConfiguration` stores platform-specific banner unit IDs outside service logic.
- If the configuration asset is absent, the project binds `DisabledAdvertisementService`, logs a diagnostic warning, and continues bootstrap without advertising.
- The current configuration uses official test banner identifiers.
- Banner lifetime follows main-menu presentation and is cleaned up on disposal.

## Android configuration

- Package: `com.tokarevdev.asteroidssurvival`
- Minimum SDK: Android API 23
- Target/compile SDK: Android API 34
- Orientation: landscape
- Google Mobile Ads: `11.2.0`
- Firebase Unity SDK: `13.15.0`

## Lifecycle and performance

- Frequently spawned enemies, projectiles, and collision effects are pooled.
- Event and SignalBus subscriptions are paired with deterministic cleanup.
- UniTask operations use owner cancellation where applicable.
- Input is read once per rendered frame by `PlayerInputStateProvider` and shared by all gameplay consumers.
- Hot loops avoid LINQ, temporary arrays, closures, and repeated component lookup.
- `HashSet` membership checks protect ordered enemy/projectile registries from duplicate registration, while entity-to-view and entity-to-damageable relationships use dictionaries for direct lookup.
- Enemy and projectile spawn paths roll registrations back and return rented objects when a later lifecycle stage fails.
- Runtime entities and Unity views are synchronized explicitly instead of sharing hidden state.
- Particle systems provide pooled collision feedback, invulnerability feedback, and speed-dependent twin thrusters.

## Scene flow

1. `Bootstrap` loads and validates JSON configuration.
2. Firebase dependencies are checked and initialized.
3. `MainMenu` opens and requests the test banner.
4. `Game` starts the survival session and logs `game_started`.
5. Player death fires SignalBus events, ends the session, pauses gameplay, logs `game_ended`, and opens the game-over UI.
6. Restart and main-menu transitions pass through the guarded navigation facade.

Enabled build-scene order: `Bootstrap`, `MainMenu`, `Game`.

## Run locally

1. Install Unity `2022.3.9f1` with the required desktop or Android build support.
2. Clone the repository with Git LFS enabled:

   ```bash
   git lfs install
   git clone https://github.com/TokarevDev/2D_Asteroids_Survival.git
   ```

3. Open the project with Unity `2022.3.9f1`.
4. Open `Assets/_Project/Scenes/Bootstrap.unity`.
5. Enter Play Mode.

Do not start from `Game.unity` when validating the full application bootstrap, Firebase initialization, advertising lifecycle, or scene navigation.

## Tech stack

- Unity `2022.3.9f1`
- C#
- Zenject and SignalBus
- UniTask `2.5.11`
- Newtonsoft.Json for Unity `3.2.2`
- UGUI and TextMeshPro
- Firebase Analytics `13.15.0`
- Google Mobile Ads `11.2.0`
- Git LFS
- Four project-level Assembly Definitions

## Visual asset provenance

The repository does not use Unity Asset Store content.

### Asteroids

Asteroid animation frames are based on [“Asteroids” by phaelax](https://opengameart.org/content/asteroids), licensed under [CC BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/). The original 16-frame sequences are imported and configured as large-asteroid and fragment visual variants in this project.

### Player ship and UFOs

The player ship and UFO sprites are based on [“Ufo's and spaceship” by dravenx](https://opengameart.org/content/ufos-and-spaceship), licensed under [CC BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/). The images were upscaled and adapted for this project. These derivative sprite files remain available under CC BY-SA 3.0.

### Backgrounds

`SpaceNebula.png`, `SpaceStars.png`, and `futuristic-moon-background.jpg` were generated specifically for this project with OpenAI ImageGen on August 20, 2026. They replace earlier web-search images whose provenance could not be verified; no unidentified stock background remains in the project.

Third-party SDKs, plugins, and fonts retain their respective vendor or open-source licenses.

## Author

Oleksandr Tokarev

Unity Developer | C# Gameplay Programmer

Email: otokarevdev@gmail.com

Portfolio: https://tokarevdev.github.io/
