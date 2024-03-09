using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class JudicatorAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Judicator");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				"Shots bounce\n" +
				"Charge shots spray a freezing blast\n" +
				string.Format("[c/78BE78:+45% damage]\n") +
				string.Format("[c/BE7878:+125% overheat use]\n") +
				string.Format("[c/BE7878:-45% speed]\n") +
				string.Format("[c/BE7878:Cannot pierce walls]\n")); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ShockCoilAddon>();
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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.JudicatorTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.beamSlotType = BeamChangeSlotID.Judicator;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageJudicator;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatJudicator;
		}


		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(30)
				.AddIngredient<Miscellaneous.EnergyShard>(30)
				.AddIngredient<Addons.IceBeamAddon>(1)
				.AddIngredient<Addons.PlasmaBeamRedAddon>(1)
				.AddIngredient(ItemID.Amethyst, 30)
				.AddTile(TileID.Hellforge)
				.Register();

			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(30)
				.AddIngredient<Miscellaneous.EnergyShard>(30)
				.AddIngredient<Addons.IceBeamAddon>(1)
				.AddIngredient<Addons.PlasmaBeamGreenAddon>(1)
				.AddIngredient(ItemID.Amethyst, 30)
				.AddTile(TileID.Hellforge)
				.Register();


		}
	}
}
