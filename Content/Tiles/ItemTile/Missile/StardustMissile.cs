using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class StardustMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Stardust Missile");
			AddMapEntry(new Color(86, 139, 163), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.StardustMissileAddon>();
			DustType = 1;
		}
	}
}
