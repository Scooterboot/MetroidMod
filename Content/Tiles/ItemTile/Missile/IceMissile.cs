using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class IceMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ice Missile");
			AddMapEntry(new Color(107, 198, 219), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.IceMissileAddon>();
			DustType = 1;
		}
	}
}
