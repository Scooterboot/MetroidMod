using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Weapons
{
	public class ChoziteCrossbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Crossbow");
			Tooltip.SetDefault("Fires two arrows in rapid succession");

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = Common.Configs.MServerConfig.Instance.damageChoziteCrossbow;
			Item.DamageType = DamageClass.Ranged;//Item.ranged = true;
			Item.width = 52;
			Item.height = 20;
			Item.useTime = 6;
			Item.useAnimation = 12;
			Item.reuseDelay = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = 15000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
			Item.shoot = ProjectileID.Shuriken;
			Item.shootSpeed = 6.7f;
			Item.useAmmo = AmmoID.Arrow;
			Item.autoReuse = true;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(7)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar",7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
