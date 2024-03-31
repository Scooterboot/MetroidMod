using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class VoltDriverAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("VoltDriver");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				"Charge shots home, explode, and confuse\n" +
				string.Format("[c/78BE78:+10% damage]\n") +
                string.Format("[c/BE7878:-10% speed]\n") +
                string.Format("[c/BE7878:+100% overheat use]\n") +
				string.Format("[c/BE7878:+10% noise]\n")); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<MagMaulAddon>();

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.VoltDriverTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.beamSlotType = BeamChangeSlotID.VoltDriver;
			mItem.addonUACost = 1f;// 400f / 120f;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageVoltDriver;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatVoltDriver;
		}


		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(15)
				.AddIngredient<Miscellaneous.EnergyShard>(2)
				.AddIngredient(ItemID.CopperBar, 10)
				.AddIngredient(ItemID.Topaz, 1)
				.AddIngredient(ItemID.Wire, 30)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(15)
				.AddIngredient<Miscellaneous.EnergyShard>(2)
				.AddIngredient(ItemID.TinBar, 10)
				.AddIngredient(ItemID.Topaz, 1)
				.AddIngredient(ItemID.Wire, 30)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
