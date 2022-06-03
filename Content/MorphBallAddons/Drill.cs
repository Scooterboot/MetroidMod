using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class Drill : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Drill/DrillItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/Drill/DrillTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball Drill");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~60% pickaxe power\n" +
			"Can mine Meteorite\n" +
			string.Format("[c/78BE78:Requires Morph Ball to use]"));
			DrillPower = 60;

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
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
