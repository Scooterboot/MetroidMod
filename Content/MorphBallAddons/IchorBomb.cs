using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class IchorBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/IchorBomb/IchorBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/IchorBomb/IchorBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/IchorBomb/IchorBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Morph Ball Bombs");
			ModProjectile.DisplayName.SetDefault("Ichor Morph Ball Bomb");
			Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Decreases enemy defense");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 30;
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.LightRed;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.IchorTorch;
			dustType2 = 170;
			dustScale = 4f;
			dustScale2 = 2f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Ichor, 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddRecipeGroup(MetroidModPorted.T2HMBarRecipeGroupID, 5)
				.AddIngredient(ItemID.Ichor, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
