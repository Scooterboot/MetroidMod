using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.weapons
{
	public class ChoziteSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Longsword");
		}
		public override void SetDefaults()
		{
			item.damage = 14;			
			item.melee = true;			
			item.width = 42;			
			item.height = 42;		
			item.useTime = 20;		
			item.useAnimation = 20;			
			item.useStyle = 1;			
			item.knockBack = 4;			
			item.value = 14000;			
			item.rare = 1;				
			item.UseSound = SoundID.Item1;	
			item.useTurn = true;	
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar",8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
