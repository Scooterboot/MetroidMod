/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.MorphBallAddons
{
	public class HallowedDrill : ModMBDrill
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/HallowedDrill/HallowedDrillItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/HallowedDrill/HallowedDrillTile";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Morph Ball Drill");
			Tooltip.SetDefault("~Left Click while morphed to drill\n" +
			"~200% pickaxe power\n" +
			"Can mine Chlorophyte");
			DrillPower = 200;

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
				.AddIngredient(ItemID.HallowedBar, 18)
				.AddIngredient(ItemID.SoulofMight, 1)
				.AddIngredient(ItemID.SoulofSight, 1)
				.AddIngredient(ItemID.SoulofFright, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}*/
