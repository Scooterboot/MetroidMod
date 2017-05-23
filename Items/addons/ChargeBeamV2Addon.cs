using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class ChargeBeamV2Addon : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Charge Beam V2";
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.toolTip = "Power Beam Addon\n" +
			"Slot Type: Charge\n" +
			"Adds Charge Effect\n" + 
			"~Charge by holding click\n" + 
			"~Charge shots deal x3 damage, but overheat x2 the normal use\n" + 
			"Allows Primare A and B combinations\n" + 
			"Converts Spazer to Wide Beam when slotted in\n" +
			"Without Spazer, the beam fires 2 shots at once\n" + 
			"Increases base damage from 7 to 10\n" + 
			"Increases base overheat use from 4 to 6";
			item.value = 2500;
			item.rare = 4;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("ChargeBeamV2Tile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChargeBeamAddon");
            recipe.AddIngredient("Soul of Might", 10);
            recipe.AddIngredient("Illegal Gun Parts");
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
