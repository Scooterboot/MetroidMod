using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace MetroidMod.Items.boss
{
	public class TorizoSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Inactive Torizo");
			Tooltip.SetDefault("Summons the Torizo");
		}
		public override void SetDefaults()
		{
			item.maxStack = 20;
			item.consumable = true;
			item.width = 48;
			item.height = 48;
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = 4;
			item.noMelee = true;
			item.value = 1000;
			item.rare = 2;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddIngredient(null, "ChozoStatueArm");
			recipe.AddIngredient(null, "ChozoStatue");
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(mod.NPCType("Torizo"));
		}
		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Torizo"));
			Main.PlaySound(15,(int)player.position.X,(int)player.position.Y,0);
			return true;
		}
	}
}
