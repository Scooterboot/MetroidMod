using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class SuperMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Super Missile");
			AddMapEntry(new Color(15, 192, 39), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.SuperMissileAddon>();
			DustType = 1;
		}
	}
}
