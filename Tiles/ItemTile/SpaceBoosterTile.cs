using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class SpaceBoosterTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Space Booster");
			AddMapEntry(new Color(79, 188, 55), name);
			drop = mod.ItemType("SpaceBooster");
			dustType = 1;
		}
	}
}