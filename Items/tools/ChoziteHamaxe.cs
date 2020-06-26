using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tools
{
	public class ChoziteHamaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Hamaxe");
		}
		public override void SetDefaults()
		{
			item.damage = 11;
			item.melee = true;
			item.width = 36;
			item.height = 36;
			item.useTime = 17;
			item.useAnimation = 20;
			item.axe = 13;	
			item.hammer = 65;	
			item.useStyle = 1;
			item.knockBack = 4;
			item.value = 12500;
			item.rare = 1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar",10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
