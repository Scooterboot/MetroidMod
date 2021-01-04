using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Bomb
{
	public class CrystalBombTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Crystal Morph Ball Bombs");
			AddMapEntry(new Color(165, 0, 181), name);
			drop = mod.ItemType("CrystalBombAddon");
			dustType = 1;
		}
	}
}