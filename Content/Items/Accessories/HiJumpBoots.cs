using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidMod.Content.Items.Accessories
{
	public class HiJumpBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hi-Jump Boots");
			// Tooltip.SetDefault("[c/ff0000:Unobtainable.] Please use the Suit Addon system.");
			/*"Increases jump height\n" + 
			"Stacks with other jump height accessories");*/

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			//Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.HiJumpBootsTile>();
		}

		/*
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(10)
				.AddIngredient<Miscellaneous.EnergyShard>(3)
				.AddIngredient(ItemID.Topaz, 1)
				.AddIngredient(ItemID.Emerald, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
		*/
		public override bool CanRightClick() => true;
		public override void RightClick(Player player)
		{
			var entitySource = player.GetSource_OpenItem(Type);

			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.HiJumpBoots>().ItemType);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<Common.Players.MPlayer>().hiJumpBoost = true;
		}
	}
}
