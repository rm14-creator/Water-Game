# Character Showcase — Unity Greybox Implementation V0.1

## Included (actual working code)
- One **runtime** showcase component that automatically constructs a greybox stage.
- Ten available profiles: Kaia, Meera, Nyx, Dagna, Vesper, Nerai, Thyra, Onyxa, Sael, SYN-01. Lucien is excluded per latest decision.
- Three activity entries per profile (30 activities total), with **three distinct procedural motion placeholder loops**.
- Three outfit choices (Work/Casual/Special), **represented by different colored greybox materials**; no production wardrobe meshes are bundled.
- Fixed / Orbit / Close-up camera selection.
- Play/Pause; persistent character, activity, outfit, camera, and playback preferences with `PlayerPrefs`.
- One-click Unity Editor scene generator.

## Setup
1. Open Unity 6.6. Create a new 3D URP project (or use a compatible project with a 3D renderer).
2. Copy `Assets/WaterGame/CharacterShowcase` into the Unity project's `Assets` directory.
3. Select **Tools > Water Game > Create Character Showcase Demo**.
4. Open `Assets/WaterGame/CharacterShowcase/Demo/CharacterShowcaseDemo.unity` if not already open.
5. Press **Play**. Select characters, activity loops, outfits, and cameras from the on-screen menu.

No external assets/packages required beyond standard Unity assemblies. Prototype UI uses IMGUI (OnGUI), intentionally avoiding dependence on custom UI Toolkit assets.

## Deliberate production gaps
This is **not the final visual showcase**. The avatars are default geometric primitives; they do not represent approved character faces, proportions, or costumes. Loops are procedural greybox motion, not authored job-specific body animation. Outfits are color swatches instead of skinned clothing. Props are simplified shapes, and professions listed as **proposed** are not canonical facts. This is a working systems demonstration.

The runtime C# implementation was authored and published, but **has not been compiled or played inside Unity** in this environment. Validate in your Unity 6.6 editor.

## Production integration guide
1. Split the static `Roster` into `ScriptableObject` assets per character, approved by the narrative director.
2. Replace `RefreshCharacter` primitive spawning with **approved character prefabs**. Keep Nyx's canon proportions, piercing, tattoos, and face; avoid photorealism.
3. Create three distinct animation clips per character, with seamless cycle boundaries. Drive through `Animator.CrossFadeInFixedTime` or an animation state machine.
4. Replace `RefreshOutfit` color changes with consistent rig-compatible `SkinnedMeshRenderer` clothing pieces, ensuring correct clipping behavior and props.
5. Implement fixed/orbit/close-up camera using Cinemachine if production needs collision control and authored blends.
6. Replace IMGUI with UI Toolkit (or your existing game UI framework) once character and asset pipelines are stable.
7. Connect unlocks and save preferences to the existing game-level save system. Keep gallery content independent of coercive colony management.
8. Test Windows and Linux builds and all outfit × animation combinations.

## Architecture and extension
Entry point: `WaterGame.Showcase.CharacterShowcase`, with editor command under `WaterGame.Showcase.Editor.ShowcaseDemoBuilder`.

The purpose of this milestone is to validate interactions, selections, state and gallery flow before investing in 30 custom animations + 30 actual outfits. All professional role assignments besides established character jobs remain **creative proposals**.
