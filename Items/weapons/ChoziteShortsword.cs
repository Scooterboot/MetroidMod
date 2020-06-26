using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.weapons
{
	public class ChoziteShortsword : ModItem
	{
		readonly int defUseTime = 10;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Shortsword");
			Tooltip.SetDefault("Right click for a backwards hop");
		}
		public override void SetDefaults()
		{
			item.damage = 14;			
			item.melee = true;			
			item.width = 32;			
			item.height = 32;
			item.useTime = item.useAnimation = defUseTime;		
			item.useStyle = 3;			
			item.knockBack = 4;			
			item.value = 12500;			
			item.rare = 1;				
			item.UseSound = SoundID.Item1;	
			item.useTurn = true;	
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.useTime = defUseTime * 3;
				if (player.itemTime == 0)
					return (true);
				return (false);
			}
			item.useTime = defUseTime;
			return (true);
		}

		public override bool UseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
                if (player.velocity.Y == 0)
                {
				    player.velocity.Y = -4f * player.gravDir;
				    player.velocity.X = -7f * player.direction;
                }
                else
                {
				    player.velocity.X = -5.5f * player.direction;
                }
				return (true);
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
