using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class FrostburnBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FrostburnBomb/FrostburnBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FrostburnBomb/FrostburnBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FrostburnBomb/FrostburnBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override double GenerationChance(int x, int y) => 20;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frostburn Morph Ball Bombs");
			ModProjectile.DisplayName.SetDefault("Frostburn Morph Ball Bomb");
			Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Frostburns enemies");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 43;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.IceTorch;
			dustType2 = DustID.IceTorch;
			dustScale2 = 3f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddRecipeGroup(MetroidMod.T1HMBarRecipeGroupID, 5)
				.AddIngredient(ItemID.FrostCore, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
