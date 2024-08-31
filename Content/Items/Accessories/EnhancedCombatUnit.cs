using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Content.Items.Addons;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Accessories
{
	public class EnhancedCombatUnit : ModItem
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
			Item.width = 32;
			Item.height = 44;
			Item.value = 1000;
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}
		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if (incomingItem.type == ModContent.ItemType<SupercooledEmblem>()|| incomingItem.type == ModContent.ItemType<HunterEmblem>() || incomingItem.type == ModContent.ItemType<FrozenCore>()) 
			{
				return false;
			}
			return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.overheatCost *= 0.85f;
			//mp.statOverheat -= 10f;
			Common.Players.HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.1f;
			Common.Players.HunterDamagePlayer.ModPlayer(player).HunterCrit += 10;
			mp.reserveTanks = 4;
			mp.reserveHeartsValue = 25;

			//mp.statOverheat -= 0.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<SupercooledEmblem>(1)
				.AddIngredient<ReserveTank5>(1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}
