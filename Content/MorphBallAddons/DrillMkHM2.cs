using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.MorphBallAddons
{
	public class DrillMkHM2 : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMkHM2/DrillMkHM2Item";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMkHM2/DrillMkHM2Tile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Drill MK5");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~165% pickaxe power\n" +
			"Can mine Adamantite and Titanium");
			DrillPower = 165;

			ItemNameLiteral = true;
		}

		public override void SetItemDefaults(Item item)
		{
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.MythrilBar, 15)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.OrichalcumBar, 18)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
