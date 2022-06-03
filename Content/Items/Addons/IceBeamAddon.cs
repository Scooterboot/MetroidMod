using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.GlobalItems;

namespace MetroidModPorted.Content.Items.Addons
{
	public class IceBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Beam");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Secondary\n" +
				"Shots freeze enemies\n" + 
				"~Each time the enemy is shot, they will become 20% slower\n" + 
				"~After 5 shots the enemy will become completely frozen\n" + 
				string.Format("[c/78BE78:+75% damage]\n") +
				string.Format("[c/BE7878:+25% overheat use]\n") +
				string.Format("[c/BE7878:-30% speed]"));
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 2500;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.IceBeamTile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 1;
			mItem.addonDmg = 0.75f;
			mItem.addonHeat = 0.25f;
			mItem.addonSpeed = -0.3f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(3)
				.AddIngredient(ItemID.IceBlock, 25)
				.AddIngredient(ItemID.Bone, 10)
				.AddIngredient(ItemID.Sapphire, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
