using MetroidMod.Common.Players;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.MorphBallAddons
{
	public class BoostBall : ModMBBoost
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/BoostBall/BoostBallItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/BoostBall/BoostBallTile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue() => true;

		public override double GenerationChance() => WorldGen.drunkWorldGen && Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues ? 20 : 7;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Boost Ball");
			/* Tooltip.SetDefault("-Hold Boost Ball Key to charge a speed boost\n" +
			"-Release the key to accelerate in the direction you are moving"); */
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Item.buyPrice(0, 1, 10, 0);
			item.rare = ItemRarityID.Orange;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<MPlayer>().BoostBall(player);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(8)
				.AddIngredient(SuitAddonLoader.GetAddon<SuitAddons.EnergyTank>().ItemType, 1)
				.AddIngredient(ItemID.Topaz, 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
