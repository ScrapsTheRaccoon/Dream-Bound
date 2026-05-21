## Dreambound
A 3D platformer built in **Unity** with **C#**, where you race through dream-like levels collecting gems and dodging falling bombs before time runs out.
> **Note:** Started from a tutorial base, then adapted to a newer Unity version, extended with custom systems (bomb mechanics, camera flipping, knockback), and more features planned.

🎮 Play it here: [Dreambound on itch.io](https://mischief-labs.itch.io/dream-bound)

## Features
- **Bomb hazard system —**
  Trigger zones spawn bombs that freefall from above, landing with a bounce and a fuse timer before detonating. A landing indicator warns you where each bomb will hit, and a shockwave knocks the player back on impact.
- **Camera flip —**
  The camera can be flipped manually with F or triggered automatically by zone triggers in the level, letting the level design play with perspective as a mechanic.
- **Gem collection & scoring —**
Gems rotate in the world and add to your score on pickup. Collect them all before the clock runs out.
- **Fall detection & death states —**
Falling off the level triggers a dedicated animation and death state, separate from the time-up state, each handled cleanly by the GameManager.
- **Animated scene transitions —**
Fade-in and fade-out panels handle transitions between the main menu, levels, and a credits scene, with a level completion popup on return to the menu.

## What I Learned
- **CharacterController-based movement —**
  Handling gravity, jumping, and rotation manually rather than relying on a Rigidbody, and layering a separate knockback velocity on top that decays and temporarily locks player input.
- **Coroutine-driven camera transitions —**
  Smoothly interpolating the camera between two offset/rotation pairs using Vector3.Lerp and Quaternion.Slerp inside a coroutine, with LateUpdate keeping it locked to the player between flips.
- **Event-driven state management —**
  GameManager broadcasts state changes (Playing, TimeUp, FellOff, LevelFinished) via a C# event, and components like PlayerControls subscribe to disable input or trigger animations without polling.
- **Singleton pattern —**
  GameManager, AudioManager, and LevelManager persist across scenes, keeping music, state, and scene loading centralised.

## Planned
- Additional obstacle types beyond bombs
- More levels

## Preview
