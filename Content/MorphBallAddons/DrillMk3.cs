using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class DrillMk3 : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMk3/DrillMk3Item";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMk3/DrillMk3Tile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Drill MK3");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~100% pickaxe power\n" +
			"Can mine Cobalt and Palladium");
			DrillPower = 100;

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
				.AddIngredient(ItemID.HellstoneBar, 20)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
