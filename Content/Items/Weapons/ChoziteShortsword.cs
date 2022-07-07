using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Weapons
{
	public class ChoziteShortsword : ModItem
	{
		private readonly int defUseTime = 10;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Shortsword");
			Tooltip.SetDefault("Right click for a backwards hop");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = Common.Configs.MServerConfig.Instance.damageChoziteShortsword;
			Item.DamageType = DamageClass.Melee;//Item.melee = true;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = Item.useAnimation = defUseTime;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.knockBack = 4;
			Item.value = 12500;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = true;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = defUseTime * 3;
				if (player.itemTime == 0)
				{
					return true;
				}
				return false;
			}
			Item.useTime = defUseTime;
			return true;
		}

		public override bool? UseItem(Player player)
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
				return true;
			}
			return base.UseItem(player);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(7)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
