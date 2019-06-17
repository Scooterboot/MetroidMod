using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.weapons
{
	public class ChoziteCrossbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Crossbow");
			Tooltip.SetDefault("Fires two arrows in rapid succession");
		}

		public override void SetDefaults()
		{
			item.damage = 12;
			item.ranged = true;
			item.width = 52;
			item.height = 20;
			item.useTime = 6;
			item.useAnimation = 12;
			item.reuseDelay = 36;
			item.useStyle = 5;
			item.noMelee = true; 
			item.knockBack = 0;
			item.value = 15000;
			item.rare = 1;
			item.UseSound = SoundID.Item5;
			item.shoot = 3;
			item.shootSpeed = 6.7f;
			item.useAmmo = AmmoID.Arrow;
			item.autoReuse = true;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar",7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
