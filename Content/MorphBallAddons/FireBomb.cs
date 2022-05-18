using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class FireBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FireBomb/FireBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FireBomb/FireBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FireBomb/FireBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(Tile tile) => true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Morph Ball Bombs");
			Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Sets enemies on fire");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 24;
			item.value = Item.buyPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.Orange;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.Torch;
			dustType2 = DustID.Torch;
			dustScale2 = 3f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
