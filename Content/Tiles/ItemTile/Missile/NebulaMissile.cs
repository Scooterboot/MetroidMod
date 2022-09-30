using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class NebulaMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Nebula Missile");
			AddMapEntry(new Color(166, 50, 150), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.NebulaMissileAddon>();
			DustType = 1;
		}
	}
}
