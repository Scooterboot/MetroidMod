using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class TropicPlating1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TropicPlating2>();
			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.TropicPlating1>());
			Item.width = 16;
			Item.height = 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(25)
				.AddIngredient(ItemID.GreenBrick, 25)
				.AddIngredient(ItemID.Emerald, 1)
				.AddTile(TileID.Furnaces)
				.Register();
			CreateRecipe(25)
				.AddIngredient(ItemID.GreenStucco, 25)
				.AddIngredient(ItemID.Emerald, 1)
				.AddTile(TileID.Furnaces)
				.Register();
		}
	}
	public class TropicPlating2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TropicPlating3>();
			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.TropicPlating2>());
			Item.width = 16;
			Item.height = 16;
		}
	}
	public class TropicPlating3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TropicPlating4>();
			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.TropicPlating3>());
			Item.width = 16;
			Item.height = 16;
		}
	}
	public class TropicPlating4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TropicPlating5>();
			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.TropicPlating4>());
			Item.width = 16;
			Item.height = 16;
		}
	}
	public class TropicPlating5 : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TropicPlating1>();
			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.TropicPlating5>());
			Item.width = 16;
			Item.height = 16;
		}
	}
}
