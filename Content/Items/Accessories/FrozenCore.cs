using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Content.Items.Addons;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Accessories
{
	public class FrozenCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supercooled Plasma Core");
			// Tooltip.SetDefault("'Strange energy core capable of producing supercooled plasma'");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 18;
			Item.height = 16;
			Item.value = 1000;
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if (incomingItem.type == ModContent.ItemType<SupercooledEmblem>() || incomingItem.type == ModContent.ItemType<HunterEmblem>() || incomingItem.type == ModContent.ItemType<EnhancedCombatUnit>())
			{
				return false;
			}
			return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
      //Multiply by what would be left over
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.overheatCost *= 0.85f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HallowedBar, 8)
				.AddIngredient(ItemID.FrostCore, 1)
				.AddIngredient(ItemID.IceBlock, 99)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
