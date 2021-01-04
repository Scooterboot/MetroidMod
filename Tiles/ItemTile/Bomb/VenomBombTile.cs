using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Bomb
{
	public class VenomBombTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Venom Morph Ball Bombs");
			AddMapEntry(new Color(165, 0, 181), name);
			drop = mod.ItemType("VenomBombAddon");
			dustType = 1;
		}
	}
}