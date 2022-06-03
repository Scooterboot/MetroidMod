using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class DrillMkHM1 : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMkHM1/DrillMkHM1Item";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMkHM1/DrillMkHM1Tile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Drill MK4");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~130% pickaxe power\n" +
			"Can mine Mythril and Orichalcum");
			DrillPower = 130;

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
				.AddIngredient(ItemID.CobaltBar, 15)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.PalladiumBar, 18)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
