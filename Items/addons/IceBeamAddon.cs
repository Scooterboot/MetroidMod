using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class IceBeamAddon : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Ice Beam";
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.toolTip = "Power Beam Addon\n" +
			"Slot Type: Secondary\n" +
			"'Mother, time to go!'\n" + 
			"Shots freeze enemies\n" + 
			"~Each time the enemy is shot, they will become 20% slower\n" + 
			"~After 5 shots the enemy will become completely frozen\n" + 
			"+100% damage\n" +
			"+25% overheat use\n" +
				"---\n" +
				"Missile Launcher Addon\n" +
				"Slot Type: Primary\n" +
				"Shots freeze enemies instantly\n" +
				"+50% damage";

			item.value = 2500;
			item.rare = 4;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("IceBeamTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient("Meteorite Bar", 3);
            recipe.AddIngredient("Snow Block", 25);
            recipe.AddIngredient("Ice Block", 10);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient("Meteorite Bar", 3);
            recipe.AddIngredient("Sapphire", 7);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
