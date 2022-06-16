using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.MorphBallAddons
{
	public class DrillMk2 : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMk2/DrillMk2Item";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/DrillMk2/DrillMk2Tile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Drill MK2");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~70% pickaxe power\n" +
			"Able to mine Hellstone");
			DrillPower = 70;

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
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 12)
				.AddRecipeGroup(MetroidMod.EvilMaterialRecipeGroupID, 6)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
