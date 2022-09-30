using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class NovaCombo : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Nova Laser");
			AddMapEntry(new Color(159, 228, 66), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.BeamCombos.NovaComboAddon>();
			DustType = 1;
		}
	}
}
