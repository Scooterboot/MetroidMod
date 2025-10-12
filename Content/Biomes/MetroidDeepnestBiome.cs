using System.Collections.Generic;
using MetroidMod.Common.Systems;
using MetroidMod.Content.NPCs.Mobs.Metroid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Biomes
{
	/// <summary>
	/// A class that only exists to give the Chozo Ruins its own music. It has little to do with actual NPC spawning, for the moment.
	/// </summary>
	public class MetroidDeepnestBiome : ModBiome
	{
		/// <summary>
		/// A list that defines what is actually allowed to spawn in the biome. See <see cref="Common.GlobalNPCs.MGlobalNPC">MGlobalNPC</see> for the backend code.
		/// </summary>
		public static List<int> AllowedNPCs = [
			ModContent.NPCType<LarvalMetroid>() // TODO: Temporary. What spawn????
		];

		public override int Music => -1; // MusicLoader.GetMusicSlot(Mod, "Assets/Music/MetroidDeepnestTheme");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

		// TODO: Bestiary sprites and stuff
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;

		// TODO: Map Background
		public override string MapBackground => base.MapBackground;
		public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<MetroidDeepnestUndergroundBackgroundStyle>();

		public override bool IsBiomeActive(Player player)
		{
			return SubworldLibrary.SubworldSystem.IsActive<Subworlds.MetroidDeepnest>() && player.ZoneRockLayerHeight;
		}
	}

	public class MetroidDeepnestUndergroundBackgroundStyle : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots)
		{
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/MetroidDeepnestUnderground0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/MetroidDeepnestUnderground1");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/MetroidDeepnestUnderground2");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/MetroidDeepnestUnderground3");
		}
	}
}
