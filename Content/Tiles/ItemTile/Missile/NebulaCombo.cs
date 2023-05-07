using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class NebulaCombo : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Nebula Singularity");
			AddMapEntry(new Color(96, 29, 133), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.BeamCombos.NebulaComboAddon>();
			DustType = 1;
		}
	}
}
