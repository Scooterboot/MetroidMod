using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Tiles.ItemTile
{
	public class PowerGripTile : ItemTile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Power Grip");
			AddMapEntry(new Color(226, 212, 49), name);
			drop = mod.ItemType("PowerGrip");
			dustType = 1;
		}
	}
}