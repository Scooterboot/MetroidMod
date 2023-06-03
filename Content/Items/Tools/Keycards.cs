using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Items.Tools
{
	public class RedKeycard : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Keycard");
			// Tooltip.SetDefault("Allows opening red hatches by right clicking");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 18;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(0, 10, 0, 0);
		}
		public override void UpdateInventory(Player player)
		{
			if (player.TryGetModPlayer(out MPlayer mp))
			{
				mp.RedKeycard = true;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.Ruby)
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	public class GreenKeycard : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Green Keycard");
			// Tooltip.SetDefault("Allows opening green hatches by right clicking");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 18;
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.buyPrice(0, 50, 0, 0);
		}
		public override void UpdateInventory(Player player)
		{
			if (player.TryGetModPlayer(out MPlayer mp))
			{
				mp.GreenKeycard = true;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.Emerald)
				.AddRecipeGroup(MetroidMod.T3HMBarRecipeGroupID, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	public class YellowKeycard : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Yellow Keycard");
			// Tooltip.SetDefault("Allows opening yellow hatches by right clicking");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 18;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(2, 5, 0, 0);
		}
		public override void UpdateInventory(Player player)
		{
			if (player.TryGetModPlayer(out MPlayer mp))
			{
				mp.YellowKeycard = true;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.Topaz)
				.AddIngredient(ItemID.LihzahrdBrick, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	public class OmniKeycard : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Omni Keycard");
			/* Tooltip.SetDefault("Allows opening all hatches by right clicking\n" +
				"'Luckily, you didn't create a MasterCard.'"); */
			// Reference to SCP: Containment Breach, wherein the process of
			// creating an Omni Card could result in a MasterCard.

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 18;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.buyPrice(12, 50, 0, 0);
		}
		public override void UpdateInventory(Player player)
		{
			if (player.TryGetModPlayer(out MPlayer mp))
			{
				mp.RedKeycard = true;
				mp.GreenKeycard = true;
				mp.YellowKeycard = true;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<RedKeycard>(1)
				.AddIngredient<GreenKeycard>(1)
				.AddIngredient<YellowKeycard>(1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
