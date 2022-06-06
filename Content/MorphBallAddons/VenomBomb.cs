using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class VenomBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VenomBomb/VenomBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VenomBomb/VenomBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VenomBomb/VenomBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venom Morph Ball Bombs");
			ModProjectile.DisplayName.SetDefault("Venom Morph Ball Bomb");
			Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Inflicts enemies with Acid Venom");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 64;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Lime;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = 171;
			dustType2 = 205;
			dustScale = 2.5f;
			dustScale2 = 2.5f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Venom, 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ItemID.VialofVenom, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
