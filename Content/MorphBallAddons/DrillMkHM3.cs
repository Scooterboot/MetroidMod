using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.MorphBallAddons
{
	public class DrillMkHM3 : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMkHM3/DrillMkHM3Item";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMkHM3/DrillMkHM3Tile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Drill MK6");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~190% pickaxe power");
			DrillPower = 190;

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
				.AddIngredient(ItemID.AdamantiteBar, 18)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.TitaniumBar, 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
