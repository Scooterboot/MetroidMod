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
			DisplayName.SetDefault("Red Keycard");
			Tooltip.SetDefault("Allows opening red hatches by right clicking");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 18;
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
			DisplayName.SetDefault("Green Keycard");
			Tooltip.SetDefault("Allows opening green hatches by right clicking");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 18;
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
			DisplayName.SetDefault("Yellow Keycard");
			Tooltip.SetDefault("Allows opening yellow hatches by right clicking");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 18;
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
			DisplayName.SetDefault("Omni Keycard");
			Tooltip.SetDefault("Allows opening all hatches by right clicking\n" +
				"'Luckily, you didn't create a MasterCard.'");
			// Reference to SCP: Containment Breach, wherein the process of
			// creating an Omni Card could result in a MasterCard.

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 18;
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
