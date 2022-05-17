using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Items.Boss
{
	public class PhantoonSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ectoplasmic Locator");
			Tooltip.SetDefault("'Gives the location of hidden ectoplasmic beings...'\n" +  
			"Summons Phantoon at night");
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
				.AddRecipeGroup(MetroidModPorted.EvilHMMaterialRecipeGroupID, 3)
				.AddIngredient(ItemID.SoulofNight, 3)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Phantoon.Phantoon>()) && !Main.dayTime;
		}
		public override bool? UseItem(Player player)
		{
			//Main.NewText("Huh, there seems to be a massive amount of ectoplasmic readings coming from... right above me!", 127, 255, 127);
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Phantoon.Phantoon>());
			SoundEngine.PlaySound(SoundID.NPCKilled, (int)player.position.X, (int)player.position.Y, 10);
			return true;
		}
	}
}
