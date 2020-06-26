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
			Tooltip.SetDefault("Right click for a lunge that deals extra knocback");
		}
		public override void SetDefaults()
		{
			item.damage = 16;			
			item.melee = true;			
			item.width = 42;			
			item.height = 42;		
			item.useTime = 20;		
			item.useAnimation = 20;			
			item.useStyle = 1;			
			item.knockBack = 5;			
			item.value = 14000;			
			item.rare = 1;				
			item.UseSound = SoundID.Item1;	
            item.autoReuse = true;
		}
        public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.useTime = 30;	
			    item.useStyle = 3;			
				if (player.itemTime == 0)
					return (true);
				return (false);
			}
			item.useTime = 20;	
			item.useStyle = 1;			
			return (true);
		}

		public override bool UseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
                if (player.velocity.Y == 0)
                {
				    player.velocity.Y = -2.75f * player.gravDir;
                    if (player.velocity.X * player.direction < 5.5f)
                    {
				        player.velocity.X = 5.5f * player.direction;
                    }
                }
                else if (player.velocity.X * player.direction < 4f)
                {
				    player.velocity.X = 4f * player.direction;
                }
				return (true);
			}
			return base.UseItem(player);
		}
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (item.useStyle == 3)
            {
                knockBack *= 1.5f;
            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			if (item.useStyle == 3 && player.velocity.X * player.direction > 0)
			{
				player.velocity.X *= -1;
            }
        }
        public override void OnHitPvp(Player player, Player target, int damage, bool crit)
        {
			if (item.useStyle == 3 && player.velocity.X * player.direction > 0)
			{
				player.velocity.X *= -1;
            }
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
