# Morph Ball Special Cases

This page implements prerequisite knowledge from [Automatic Content Generation]. Please read that documentation page for a better understanding of the system at hand.

Due to a limitation with how [IGeneratesModProjectile], Specials need a specific system to delineate between the Power Bomb's "Bomb" and "Explosion" projectiles. Bombs have recieved the same system because it helped prototype the Power Bomb's system.

## What in god's name am I reading?

This was the most modder-friendly approach I could think of. May god have mercy on my soul.

Basically, two subclasses of a given [ModMorphBallSpecial] are created. Both extend [IGeneratesModProjectile]: one for the Bomb, and one for the Explosion. These subclasses both pass some functionality up the chain to the parent class for modder customization, but mostly manage things on their own.

[Automatic Content Generation]: /Documentation/AutomaticContentGeneration.md
[IGeneratesModProjectile]: /Content/Projectiles/GeneratedModProjectile.cs
[ModMorphBallSpecial]: /Content/MorphBallAddons/ModMorphBallSpecial.cs