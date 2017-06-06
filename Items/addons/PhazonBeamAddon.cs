using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class PhazonBeamAddon : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Phazon Beam";
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.toolTip = "Power Beam Addon\n" +
			"Slot Type: Charge\n" +
			"'It's made of pure Phazon energy!'\n" + 
			"Shots have a 25% chance of inflicting a Phazon DoT debuff on enemies\n" + 
			"Only activates when the Secondary, Utility, and Primary A and B slots are in use\n" +
			"Cannot be used without the Phazon Suit";
			item.value = 2500;
			item.rare = 4;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("PhazonBeamTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Shroomite Bar", 6);
            recipe.AddIngredient(null, "PurePhazon", 12);
            recipe.AddIngredient("Soul of Night", 5);
			recipe.AddIngredient("Soul of Sight", 10);
			recipe.AddIngredient("Soul of Might", 10);
			recipe.AddIngredient("Soul of Fright", 10);
            recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
