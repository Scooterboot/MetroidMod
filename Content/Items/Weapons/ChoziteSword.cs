using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Weapons
{
	public class ChoziteSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Longsword");
			Tooltip.SetDefault("Right click for a lunge that deals extra knocback");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;//Item.melee = true;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
			Item.value = 14000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = 30;	
				Item.useStyle = ItemUseStyleID.Thrust;
				if (player.itemTime == 0)
				{
					return true;
				}
				return false;
			}
			Item.useTime = 20;	
			Item.useStyle = ItemUseStyleID.Swing;			
			return true;
		}

		public override bool? UseItem(Player player)
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
				return true;
			}
			return base.UseItem(player);
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (Item.useStyle == 3)
			{
				knockBack *= 1.5f;
			}
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Item.useStyle == ItemUseStyleID.Thrust && player.velocity.X * player.direction > 0)
			{
				player.velocity.X *= -1;
			}
		}
		public override void OnHitPvp(Player player, Player target, int damage, bool crit)
		{
			if (Item.useStyle == ItemUseStyleID.Thrust && player.velocity.X * player.direction > 0)
			{
				player.velocity.X *= -1;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(8)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar",8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
