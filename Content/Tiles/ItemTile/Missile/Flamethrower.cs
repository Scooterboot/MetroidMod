using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class Flamethrower : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Flamethrower");
			AddMapEntry(new Color(243, 162, 63), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.BeamCombos.FlamethrowerAddon>();
			DustType = 1;
		}
	}
}
