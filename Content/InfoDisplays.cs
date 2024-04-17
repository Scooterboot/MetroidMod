using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content
{
	internal class ExpenseDisplay : InfoDisplay
	{
		public override void SetStaticDefaults()
		{
			// InfoName.SetDefault("Barrier Resilliency (ExpEff)");
		}

		public override bool Active() => MetroidMod.DisplayDebugValues;

		public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
		{
			float efficiency = Main.LocalPlayer.MetroidPlayer().EnergyExpenseEfficiency * 100f;
			return $"Barrier Resilliency: {efficiency:F2}";
		}
	}
}
