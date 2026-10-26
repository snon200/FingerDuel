# Finger Duel

Local two-player Unity game for a shared phone or tablet. Players sit on opposite sides of the screen (green vs red), each dragging a physics rope to hit the other. First to knock the opponent out of health wins.

## Links

- [Snon Apps on Google Play](https://play.google.com/store/apps/dev?id=8134158059265445905) (Finger Duel is published under this developer account)

## Gameplay

- Split-screen, same-device play (no online matchmaking)
- Each player has three hit points
- Rope colliders deal knockback; the last remaining player wins
- After a round, the game returns to the menu

## Project

- Engine: Unity 2023.2
- Company: SnonApps
- Product name: Finger Duel
- Android application ID: `com.SnonApps.FingerDuel`
- Main scripts: `Assets/Scripts/` (`GameScript`, `PlayerScript`, `Rope`, `MultipleTouch`, `Menu`)

## Open in Unity

1. Install Unity 2023.2.20f1 (or a compatible 2023.2 editor).
2. Open this folder as a Unity project.
3. Load the menu scene from `Assets/Scenes/` and press Play.

Android signing keystores and Unity `Library/` output are not in this repo.
