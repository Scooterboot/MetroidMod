using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class IceSuperMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ice Super Missile");
			AddMapEntry(new Color(42, 120, 213), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.IceSuperMissileAddon>();
			DustType = 1;
		}
	}
}
