using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class VortexCombo : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Vortex Storm");
			AddMapEntry(new Color(38, 148, 144), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.BeamCombos.VortexComboAddon>();
			DustType = 1;
		}
	}
}
