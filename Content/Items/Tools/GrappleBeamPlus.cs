using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace MetroidMod.Content.Items.Tools
{
	public class GrappleBeamPlus : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Grappling Beam");
			/* Tooltip.SetDefault("'Swingy!'\n" + 
			"Press left or right to swing\n" + 
			"Press up or down to ascend or descend the grapple\n" +
			"Will only attach to specific tiles"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.AmethystHook);

			Item.width = 20;
			Item.height = 20;
			Item.value = 20000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = Sounds.Items.Tools.GrappleBeamSound;
			Item.shoot = ModContent.ProjectileType<Projectiles.GrappleBeamPlusShot>();
			Item.shootSpeed = 12f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(15)
				.AddIngredient<Miscellaneous.EnergyShard>(3)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod); 
			recipe.AddIngredient(null, "ChoziteBar", 15);
			recipe.AddIngredient(null, "EnergyShard", 3);
			recipe.AddTile(TileID.Anvils);   
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
