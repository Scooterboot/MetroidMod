using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class EnergyTank : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Energy Tank");
			// Tooltip.SetDefault("[c/ff0000:Unobtainable.] Please use the Suit Addon system.");

			Item.ResearchUnlockCount = 10;
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			//Item.consumable = true;
			//Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.EnergyTank>();
			Item.rare = ItemRarityID.Green;
			Item.value = 1000;
		}
		public override bool CanRightClick() => true;
		public override void RightClick(Player player)
		{
			var entitySource = player.GetSource_OpenItem(Type);

			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.EnergyTank>().ItemType);
		}
	}
}
