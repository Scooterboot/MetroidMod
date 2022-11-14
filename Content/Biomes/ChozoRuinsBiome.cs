using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace MetroidMod.Content.Biomes
{
	/// <summary>
	/// A class that only exists to give the Chozo Ruins its own music. It has little to do with actual NPC spawning, for the moment.
	/// </summary>
	public class ChozoRuinsBiome : ModBiome
	{
		public override int Music => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo) ? MusicLoader.GetMusicSlot(Mod, "Assets/Music/ChozoRuinsActive") : MusicLoader.GetMusicSlot(Mod, "Assets/Music/ChozoRuinsInactive");
		public override SceneEffectPriority Priority => SceneEffectPriority.Event;

		// TODO: Bestiary sprites and stuff
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;

		// TODO: Map Background
		public override string MapBackground => base.MapBackground;

		public override bool IsBiomeActive(Player player)
		{
			if (ModContent.GetInstance<MBiomesSystem>().chozoBlockCount >= 100)
			{
				int num = (int)player.Center.X / 16;
				int num2 = (int)player.Center.Y / 16;
				if (Main.tile[num, num2] != null && Main.tile[num, num2].WallType == ModContent.WallType<Walls.ChozoBrickWallNatural>())
				{
					return true;
				}
			}
			return false;
		}
	}
}
