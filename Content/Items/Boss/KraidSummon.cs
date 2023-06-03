using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Boss
{
	public class KraidSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Variation Matrix");
			// Tooltip.SetDefault("Summons Kraid");

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
				.AddRecipeGroup(MetroidMod.T1HMBarRecipeGroupID, 1)
				.AddIngredient(ItemID.PixieDust, 3)
				.AddTile(MUtils.CalamityActive() ? TileID.Anvils : TileID.MythrilAnvil)
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
			return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Kraid.Kraid_Head>());
		}
		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Kraid.Kraid_Head>());
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return true;
		}
	}
}
