# MiniDungeon — 2D Platformer in Unity (In progress)

A 2D platformer game featuring two playable classes, Hero (sword) and Mage (projectiles). It features enemies with custom movement, a health bar, damage feedback, and a Game Over screen.

---

## Index
1. Features
2. Requirements
3. Controls
4. Script Structure
5. Future Develpments

---

## 1. Features
- **Two classes on the same Player**, selected via a pause menu dropdown. The default is Hero. Each class has its own health, damage, cooldown, and invincibility time.
- **Hero**: Forward-facing cone sword strike, activated with Shift, featuring a sweep animation. Damage only affects targets inside the cone.
- **Mage**: Fires projectiles in a straight line in the direction the player is facing.
- **Slime Enemy**: Patrols left and right by hopping and deals contact damage. The player passes through it but takes damage.
- **Reusable enemy movement**: Implemented as a separate component, allowing any future enemy to use the same movement or a new one.
- **Health bar in the top-left corner**, displaying the health of the currently active class.
- **Damage feedback**: The sprite flashes red, and the player becomes invincible for half a second.
- **Game Over** screen with a restart button.
- **Pause menu** that freezes the game.

---

## 2. Requirements

| Requirement |
| :--- |
| **Unity 6 (6000.x)** |
| **Input System** |
| **TextMeshPro** |

---

## 3. Controls
Movement, jump, and pause controls are the bindings you define in your Input Actions asset. Attacking uses the Shift key.

| Action | Input Action Type | Binding |
| :--- | :--- | :--- |
| **Move** | Value, Axis (float) | A (Right) D (Left) |
| **Jump** | Button | Spacebar |
| **Attack** | Button | Left Shift |
| **Pause** | Button | P |

---

## 4. Script Structure

### Characters (Player)
| Script | Responsibility |
| :--- | :--- |
| `Character.cs` | Abstract base class for the player. Handles `currentHp`, `TakeDamage()` (with invincibility), `Die()`, and the `Died` event. |
| `Hero.cs` | Hero class: specific stats and a cone sword attack. |
| `Mage.cs` | Mage class: specific stats and projectile shooting. |
| `Projectile.cs` | Mage's projectile: moves in a straight line, deals damage, and destroys itself. |
| `PlayerMovement.cs` | Walking, jumping (variable height), and flipping the character. |
| `PlayerAttack.cs` | Connects the attack input to whatever class component is currently active. |
| `PlayerClassSelector.cs` | Enables and disables Hero and Mage based on the dropdown selection. |
| `DamageFlash.cs` | Makes the sprite flash red when taking damage (player and enemies). |

### Enemies
| Script | Responsibility |
| :--- | :--- |
| `Enemy.cs` | Abstract base class for enemies: health, `TakeDamage()`, and `Die()`. |
| `Slime.cs` | Enemy that deals contact damage to the player. |
| `EnemyMovement.cs` | Abstract base class for movement: patrol, ground detection, and turning at walls. |
| `HopMovement.cs` | Hopping movement, configurable in the Inspector. |

### Common, UI, and Game
| Script | Responsibility |
| :--- | :--- |
| `IDamageable.cs` | Interface containing `TakeDamage()`. Implemented by anything that can take damage. |
| `PlayerHealthUI.cs` | Health bar and text for the active class. |
| `PauseMenu.cs` | Opens and closes the pause panel and freezes time. |
| `GameOverUI.cs` | Displays the Game Over panel and restarts the scene. |

---

## 5. Future Develpments
- New Enemy Archetypes
- Advanced Platforming Mechanics
- Checkpoint & Respawn System
- World Building
- Account creations and Save / Load System
- Collectibles & Economy
- Full Sprite Animations
- Particle Effects
- Audio Manager