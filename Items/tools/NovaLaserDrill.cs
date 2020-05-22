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
			Tooltip.SetDefault("Capable of mining Phazon");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 16;
			item.scale = 0.8f;
			item.useStyle = 5;
			item.useAnimation = 15;//25;
			item.useTime = 4;//7;
			item.pick = 215;
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
			recipe.AddIngredient(ItemID.Picksaw);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddIngredient(ItemID.CursedFlame, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Picksaw);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddIngredient(ItemID.Ichor, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}