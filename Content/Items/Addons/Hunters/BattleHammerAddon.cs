using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class BattleHammerAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("BattleHammer");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				"Shots explode\n" +
				string.Format("[c/BE7878:+100% overheat use]\n") +
				string.Format("[c/BE7878:-40% speed]\n") +
                string.Format("[c/BE7878:Cannot pierce walls or enemies]")); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<VoltDriverAddon>();
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 50000;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.BattleHammerTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonUACost = 400f / 150f;
			mItem.beamSlotType = BeamChangeSlotID.BattleHammer;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageBattleHammer;
		}


		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.GoldBar, 8)
				.AddIngredient<Miscellaneous.EnergyShard>(2)
				.AddIngredient<Miscellaneous.ChoziteBar>(15)
				.AddIngredient(ItemID.JungleSpores, 15)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.Grenade, 20)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.PlatinumBar, 8)
				.AddIngredient<Miscellaneous.EnergyShard>(2)
				.AddIngredient<Miscellaneous.ChoziteBar>(15)
				.AddIngredient(ItemID.JungleSpores, 15)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.Grenade, 20)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
