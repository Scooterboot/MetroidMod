using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class CursedFlameBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CursedFlameBomb/CursedFlameBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CursedFlameBomb/CursedFlameBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CursedFlameBomb/CursedFlameBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override double GenerationChance(int x, int y) => 20;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cursed Fire Morph Ball Bombs");
			// ModProjectile.DisplayName.SetDefault("Cursed Fire Morph Ball Bomb");
			/* Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Burns enemies with Cursed Flames"); */
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 51;
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.LightRed;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.CursedTorch;
			dustType2 = DustID.CursedTorch;
			dustScale2 = 3f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.CursedInferno, 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddRecipeGroup(MetroidMod.T2HMBarRecipeGroupID, 5)
				.AddIngredient(ItemID.CursedFlame, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
