using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.weapons
{
	public class ChoziteShortsword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Shortsword");
			Tooltip.SetDefault("Right click to do a small jump backwards");
		}

		public override void SetDefaults()
		{
			item.damage = 12;			
			item.melee = true;			
			item.width = 32;			
			item.height = 32;		
			item.useTime = 10;		
			item.useAnimation = 10;			
			item.useStyle = 3;			
			item.knockBack = 4;			
			item.value = 1700;			
			item.rare = 1;				
			item.UseSound = SoundID.Item1;	
			item.useTurn = true;	
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool UseItem(Player player)
		{
			if(player.altFunctionUse == 2 && player.velocity.Y == 0)
			{
				player.velocity.Y = -4f;
				player.velocity.X = -7f * player.direction;
			}
			return base.UseItem(player);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}
