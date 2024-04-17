using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Addons.Hunters
{
	public class ImperialistAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Imperialist");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Charge\n" +
				string.Format("[c/78BE78:+500% damage]\n") +
				string.Format("[c/78BE78:Adds scope and stealth]\n") +
				string.Format("[c/BE7878:+500% overheat use]\n") +
				string.Format("[c/BE7878:Massive speed reduction]\n")); */
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<JudicatorAddon>();
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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.Hunters.ImperialistTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 0;
			mItem.addonUACost = 400f / 30f;
			mItem.beamSlotType = BeamChangeSlotID.Imperialist;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageImperialist;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatImperialist;
		}


		public override void AddRecipes()
		{
			/*CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(30)
				.AddIngredient<Miscellaneous.EnergyShard>(30)
				.AddIngredient(ItemID.ZapinatorGray, 1)
				.AddIngredient(ItemID.SoulofFright, 20)
				.AddIngredient(ItemID.Ruby, 20)
				.AddTile(TileID.Hellforge)
				.Register();

			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(30)
				.AddIngredient<Miscellaneous.EnergyShard>(30)
				.AddIngredient(ItemID.ZapinatorOrange, 1)
				.AddIngredient(ItemID.SoulofFright, 20)
				.AddIngredient(ItemID.Ruby, 20)
				.AddTile(TileID.Hellforge)
				.Register();*/

			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(15)
				.AddIngredient<Miscellaneous.EnergyShard>(2)
				.AddIngredient(ItemID.BlackLens, 2)
				.AddIngredient(ItemID.Ruby, 1)
				.AddIngredient(ItemID.IllegalGunParts, 1)
				.AddTile(TileID.Anvils)
				.Register();

		}
	}
}
