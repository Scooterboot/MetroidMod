using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class HomingMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Homing Missile");
			AddMapEntry(new Color(173, 0, 82), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.HomingMissileAddon>();
			DustType = 1;
		}
	}
}
