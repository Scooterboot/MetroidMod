using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Tools;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class SpiderBall : ModMorphBallAddon, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SpiderBall/SpiderBallItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SpiderBall/SpiderBallTile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues
				? 20
				: 15
				;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spider Ball");
			/* Tooltip.SetDefault("-Press the Spider Ball Key to activate Spider Ball\n" +
			"-Allows you to climb on walls and ceilings"); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<GrappleBeam>();
		}
		public override void ItemSetDefaults()
		{
			Item.value = Item.buyPrice(0, 0, 90, 0);
			Item.rare = ItemRarityID.Orange;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<MPlayer>().SpiderBall(player);
		}
		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				.AddIngredient(null, "ChoziteBar", 12)
				.AddRecipeGroup(MetroidMod.PreHMhooksRecipeID, 1)
				.AddIngredient(ItemID.Silk, 50)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
