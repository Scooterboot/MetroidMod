using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class DiffusionMissile : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Diffusion Missile");
			AddMapEntry(new Color(255, 0, 90), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.DiffusionMissileAddon>();
			DustType = 1;
		}
	}
}
