Nerd Helper
===========

[Download Nerd Helper here](https://gamebanana.com/mods/338413)

A helper mod containing custom entities for Celeste maps, supporting both Ahorn and Lönn. Nerd Helper currently contains the following entities:

* **Dash Through Spikes**: They're spikes that you can dash through. That's it. The texture is meant to evoke dream blocks, but they can be easily retextured via the map editors. If you attach them to dream blocks, I recommend ticking the Below attribute on the dream block to make its edges render behind the spikes. If you set them to only prevent deaths at 0 speed, they function as **Jumpthru Fixer Spikes** that allow you to start a dash directly inside them without dying.
* **Bouncy Jellyfish**: Jellyfish that you can bounce off the top of.
* **Jump Swap Block**: Swap Block triggered by jumping instead of dashing.
* **Noded Fling Bird**: Fling Bird that uses nodes instead of multiple placements to control its future positions. Also supports both throwing directions.
* **Noded Fling Bird Skip Trigger**: Trigger that lets a Noded Fling Bird skip its positions.
* **Cutscene Screen Shake Trigger**: Trigger that causes screen shake which can be enabled by a setting separate from the vanilla one.

## Cutscene Screen Shake
Cutscene Screen Shake can also be used from C# code via [ModInterop](https://github.com/EverestAPI/Resources/wiki/Cross-Mod-Functionality#modinterop) (via [this interop class](https://github.com/Kalobi/NerdHelper/blob/master/Code/Module/NerdHelperInterop.cs)) or from Lua Cutscenes as follows:
```lua
--at the top
local nerdhelperutils = require("#Celeste.Mod.NerdHelper.Module.NerdHelperInterop")

--in your cutscene
nerdhelperutils.CutsceneShake(0.3)
nerdhelperutils.CutsceneDirectionalShake(vector2(1,1), 0.5)
```

## Noded Fling Birds
Noded Fling Birds differ from normal Fling Birds in the following ways:
* Nodes are used to mark future positions instead of controlling the flight path. Multiple placed birds will be completely separate chains.
* The fling direction (right or left) can be individually chosen for each node.
* The camera zoom effect and the mechanic of waiting for lightning to disappear at the end can be toggled.
* Individual nodes cannot be set to "Waiting".
* The bird does not disappear depending on the player position when entering the room.
* The bird does not automatically skip forward when the player moves far enough to the right, since the bird can follow an arbitrary path. Instead, you can use a **Noded Fling Bird Skip Trigger** to force the bird to travel forward to a specific node when the trigger is reached.

That's everything for now, but more will probably follow. If you find any issues or have suggestions for improvement, you can submit an issue.
