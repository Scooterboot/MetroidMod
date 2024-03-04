using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Accessories
{
	public class PowerGrip : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Power Grip");
			// Tooltip.SetDefault("[c/ff0000:Unobtainable.] Please use the Suit Addon system.");//"Allows the user to grab onto ledges\n" + "Does not need to be equipped; works while in inventory");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			//Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.PowerGripTile>();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.powerGrip = true;
		}
		public override void UpdateInventory(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.powerGrip = true;
		}
		public override void UpdateVanity(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.powerGrip = true;
		}
		/*
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(4)
				.AddIngredient(ItemID.Sapphire, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
		*/
		public override bool CanRightClick() => true;
		public override void RightClick(Player player)
		{
			var entitySource = player.GetSource_OpenItem(Type);

			player.QuickSpawnItem(entitySource, SuitAddonLoader.GetAddon<SuitAddons.PowerGrip>().ItemType);
		}
	}
}
