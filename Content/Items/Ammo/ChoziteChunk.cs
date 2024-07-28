using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Miscellaneous;
using MetroidMod.Content.SuitAddons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Ammo
{
	public class ChoziteChunk : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.knockBack = 1.5f;
			Item.value = 10;
			Item.shoot = ModContent.ProjectileType<Projectiles.ChoziteChunk>();
			Item.shootSpeed = 4f;
			Item.ammo = AmmoID.Sand;
		}
		public override void AddRecipes() {
			CreateRecipe(15)
				.AddIngredient<ChoziteBar>()
				.AddIngredient(ItemID.SandBlock, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
