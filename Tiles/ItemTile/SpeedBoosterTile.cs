using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class SpeedBoosterTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Speed Booster");
			AddMapEntry(new Color(79, 188, 55), name);
			drop = mod.ItemType("SpeedBooster");
			dustType = 1;
		}
	}
}