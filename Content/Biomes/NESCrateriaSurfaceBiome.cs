using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace MetroidMod.Content.Biomes
{
	public class NESCrateriaSurfaceBiome : ModBiome
	{
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<NESCrateriaSurfaceBackgroundStyle>();
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crateria (Surface, Metroid NES)");
		}

		public override bool IsBiomeActive(Player player)
		{
			bool b1 = ModContent.GetInstance<MBiomesSystem>().nesCrateriaBlockCount >= 50;
			bool b2 = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return b1 && b2;
		}
	}
}
