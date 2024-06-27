Nerd Helper
===========

[Download Nerd Helper here](https://gamebanana.com/mods/338413)

A helper mod containing custom entities for Celeste maps, supporting both Ahorn and Lönn. Nerd Helper currently contains the following entities:

* **Dash Through Spikes**: They're spikes that you can dash through. That's it. The texture is meant to evoke dream blocks, but they can be easily retextured via the map editors. If you attach them to dream blocks, I recommend ticking the Below attribute on the dream block to make its edges render behind the spikes. If you set them to only prevent deaths at 0 speed, they function as **Jumpthru Fixer Spikes** that allow you to start a dash directly inside them without dying.
* **Bouncy Jellyfish**: Jellyfish that you can bounce off the top of.
* **Jump Swap Blocks**: Swap Blocks triggered by jumping instead of dashing.
* **Cutscene Screen Shake Trigger**: Trigger that causes screen shake which can be enabled by a setting separate from the vanilla one.

Cutscene Screen Shake can also be used from C# code via [ModInterop](https://github.com/EverestAPI/Resources/wiki/Cross-Mod-Functionality#modinterop) (via [this interop class](https://github.com/Kalobi/NerdHelper/blob/master/Code/Module/NerdHelperInterop.cs)) or from Lua Cutscenes as follows:
```lua
--at the top
local nerdhelperutils = require("#Celeste.Mod.NerdHelper.Module.NerdHelperInterop")

--in your cutscene
nerdhelperutils.CutsceneShake(0.3)
nerdhelperutils.CutsceneDirectionalShake(vector2(1,1), 0.5)
```

That's everything for now, but more will probably follow. If you find any issues or have suggestions for improvement, you can submit an issue.
