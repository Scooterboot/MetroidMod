using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class HomingMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Homing Missile");
			AddMapEntry(new Color(173, 0, 82), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.HomingMissileAddon>();
			DustType = 1;
		}
	}
}
