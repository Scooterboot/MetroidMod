using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tools
{
	public static class ChoziteDualtoolSettings
	{
		public static bool IsPlacing = true;
		public static bool ApplyRegen = true;
		public static bool AllowPlaceNew = true;
		public static bool AllowPlaceOnEmpty = true;

		public static bool CanShow => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ChoziteDualtool>();
	}
}

