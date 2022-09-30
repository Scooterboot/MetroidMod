using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class Wavebuster : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wavebuster");
			AddMapEntry(new Color(92, 58, 156), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.BeamCombos.WavebusterAddon>();
			DustType = 1;
		}
	}
}
