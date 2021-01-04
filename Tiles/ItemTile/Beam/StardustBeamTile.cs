using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile.Beam
{
	public class StardustBeamTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Stardust Beam");
			AddMapEntry(new Color(35, 200, 254), name);
			drop = mod.ItemType("StardustBeamAddon");
			dustType = 1;
		}
	}
}