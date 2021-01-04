using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Bomb
{
	public class IchorBombTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ichor Morph Ball Bombs");
			AddMapEntry(new Color(165, 0, 181), name);
			drop = mod.ItemType("IchorBombAddon");
			dustType = 1;
		}
	}
}