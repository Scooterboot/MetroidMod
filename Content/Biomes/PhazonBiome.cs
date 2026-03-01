using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Biomes
{
	public class PhazonBiome : ModBiome
	{
		//public override ModWaterStyle WaterStyle => ModContent.GetInstance<ExampleWaterStyle>();
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/PhazonBiome");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

		// TODO: Bestiary sprites and stuff
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;

		// TODO: Map Background
		public override string MapBackground => base.MapBackground;

		public override bool IsBiomeActive(Player player)
		{
			bool b1 = ModContent.GetInstance<MBiomesSystem>().phazonBlockCount >= 50;
			bool b2 = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return b1 && b2;
		}
	}
}
