using Terraria;
using Terraria.ID;
using MetroidModPorted.Common.Players;
using MetroidModPorted.ID;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class SpiderBall : ModMBUtility
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SpiderBall/SpiderBallItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SpiderBall/SpiderBallTile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(Tile tile) => true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spider Ball");
			Tooltip.SetDefault("-Press the Spider Ball Key to activate Spider Ball\n" +
			"-Allows you to climb on walls and ceilings");
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 0, 90, 0);
			item.rare = ItemRarityID.Orange;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<MPlayer>().SpiderBall(player);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MetroidModPorted.T1HMBarRecipeGroupID, 8)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.SpiderFang, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
