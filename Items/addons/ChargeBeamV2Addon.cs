using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class ChargeBeamV2Addon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charge Beam V2");
			Tooltip.SetDefault("Power Beam Addon\n" +
			"Slot Type: Charge\n" +
			"Adds Charge Effect\n" + 
			"~Charge by holding click\n" + 
			"~Charge shots deal x3 damage, but overheat x2 the normal use\n" + 
			"Converts Spazer to Wide Beam when slotted in\n" +
			"Without Spazer, the beam fires 2 shots at once\n" + 
			"Increases base damage from 7 to 10\n" + 
			"Increases base overheat use from 4 to 6");
		}
		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.value = 2500;
			item.rare = 4;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("ChargeBeamV2Tile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.addonSlotType = 0;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChargeBeamAddon");
            		recipe.AddIngredient(ItemID.SoulofMight, 10);
            		recipe.AddIngredient(ItemID.IllegalGunParts);
            		recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
