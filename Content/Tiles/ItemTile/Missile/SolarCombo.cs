using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class SolarCombo : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Supernova");
			AddMapEntry(new Color(184, 124, 75), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.BeamCombos.SolarComboAddon>();
			DustType = 1;
		}
	}
}
