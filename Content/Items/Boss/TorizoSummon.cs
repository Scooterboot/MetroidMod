using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace MetroidMod.Content.Items.Boss
{
	public class TorizoSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Inactive Torizo");
			// Tooltip.SetDefault("Summons the Torizo");

			Item.ResearchUnlockCount = 3;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 20;
			Item.consumable = true;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
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
		
		public override bool ConsumeItem(Player player)
		{
			if (Common.Configs.MConfigMain.Instance.enableBossSummonConsumption)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Torizo.Torizo>());
		}
		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Torizo.Torizo>());
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return true;
		}
	}
}
