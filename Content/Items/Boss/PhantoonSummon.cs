using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Boss
{
	public class PhantoonSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ectoplasmic Locator");
			/* Tooltip.SetDefault("'Gives the location of hidden ectoplasmic beings...'\n" +  
			"Summons Phantoon at night"); */

			Item.ResearchUnlockCount = 3;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 20;
			Item.consumable = true;
			Item.width = 12;
			Item.height = 12;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = 4;
			Item.noMelee = true;
			Item.value = 1000;
			Item.rare = 7;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
				.AddRecipeGroup(MetroidMod.EvilHMMaterialRecipeGroupID, 3)
				.AddIngredient(ItemID.SoulofNight, 3)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

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
			return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Phantoon.Phantoon>()) && !Main.dayTime;
		}
		public override bool? UseItem(Player player)
		{
			//Main.NewText("Huh, there seems to be a massive amount of ectoplasmic readings coming from... right above me!", 127, 255, 127);
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Phantoon.Phantoon>());
			SoundEngine.PlaySound(SoundID.NPCDeath10, player.position);
			return true;
		}
	}
}
