using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.tools
{
	public class NovaLaserDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Laser Drill");
			//Tooltip.SetDefault("Capable of mining Phazon");
			Tooltip.SetDefault("This is the only tool that can mine Phazon");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 16;
			item.scale = 0.8f;
			item.useStyle = 5;
			item.useAnimation = 4;
			item.useTime = 4;
			item.pick = 200;//215;
			item.damage = 35;
			item.knockBack = 3f;
			item.value = 15000;
			item.rare = 2;
			item.shoot = mod.ProjectileType("NovaLaserDrillShot");
			item.UseSound = SoundID.Item23;
			item.tileBoost = 10;
			item.melee = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.Picksaw);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddIngredient(ItemID.CursedFlame, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.Picksaw);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddIngredient(ItemID.Ichor, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override void HoldItem(Player player)
		{
			bool flag13 = player.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && player.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY;
			if (player.noBuilding)
			{
				flag13 = false;
			}
			Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
			if (flag13 && tile != null && tile.active() && !player.mouseInterface && (tile.type == mod.TileType("PhazonTile") || tile.type == mod.TileType("PhazonCore")))
			{
				item.pick = 1000;
			}
			else
			{
				item.pick = 200;
			}
		}
	}
}