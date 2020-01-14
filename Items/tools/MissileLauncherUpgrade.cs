using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace MetroidMod.Items.tools
{
	public class MissileLauncherUpgrade : ModItem
	{
		public override void SetDefaults()
		{
			item.width = item.height = 16;

			item.useStyle = 4;
			item.useTime = item.useAnimation = 15;

			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();

			if (mp.missileLauncherUpgrade)
				return (false);
			return (true);
		}

		public override bool UseItem(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();

			mp.missileLauncherUpgrade = true;

			Main.NewText("You have unlocked the missile launcher.", Color.Red);
			Main.NewText("Right click with the Power Beam selected to switch.", Color.Red);
			return (true);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 10);
			recipe.AddIngredient(null, "EnergyTank", 1);
			recipe.AddIngredient(ItemID.Musket, 1);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 10);
			recipe.AddIngredient(null, "EnergyTank", 1);
			recipe.AddIngredient(ItemID.TheUndertaker, 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
