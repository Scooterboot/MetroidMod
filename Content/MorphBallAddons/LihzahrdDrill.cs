/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.MorphBallAddons
{
	public class LihzahrdDrill : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/LihzahrdDrill/LihzahrdDrillItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/LihzahrdDrill/LihzahrdDrillTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Morph Ball Drill");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~210% pickaxe power\n" +
			"Capable of mining Lihzahrd Bricks");
			DrillPower = 210;

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
				.AddIngredient(ItemID.LihzahrdBrick, 50)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}*/
