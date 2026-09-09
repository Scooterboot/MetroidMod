# Automatic Content Generation

### What?

[IGeneratesModItem] and [IGeneratesModTile] are both interfaces that link directly to their respective generated modtype, those being a ModItem and ModTile respectively. In the end, any class that implements these interfaces will automatically create their respective modtypes and anything extending the main class will be able to easily interface with the modtype.

### Why?

This eases content creation for things like [Suit Addons] while creating common code for a given modtype. We don't need to have separate implementations for many different things that all end up having similar properties and associated modtypes.

## How

When a class implements [IGeneratesModItem], it is expected to create a [GeneratedModItem] (and feed itself into it) in its Load() method, as well as feeding it into Mod.AddContent(ILoadable) and feeding the [GeneratedModItem].Type integer into ItemType.

By feeding it into Mod.AddContent, we allow tModLoader to update the item instead of our framework doing it on its own.

The same applies to [IGeneratesModTile]; just replace the word Item with Tile.

### Utilization

Following the usual "class implements interface" requirements and the requirements above will result in a custom item/tile tied to a ModType. Enjoy!


[IGeneratesModItem]: /Content/Items/GeneratedModItem.cs
[IGeneratesModTile]: /Content/Tiles/GeneratedModTile.cs

[GeneratedModItem]: /Content/Items/GeneratedModItem.cs

[Suit Addons]: /Content/SuitAddons/ModSuitAddon.cs
