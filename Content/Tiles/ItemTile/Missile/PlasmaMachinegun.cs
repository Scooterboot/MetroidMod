using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class PlasmaMachinegun : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Plasma Machinegun");
			AddMapEntry(new Color(15, 192, 39), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.BeamCombos.PlasmaMachinegunAddon>();
			DustType = 1;
		}
	}
}
