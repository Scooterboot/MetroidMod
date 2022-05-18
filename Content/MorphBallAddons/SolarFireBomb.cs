using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class SolarFireBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarFireBomb/SolarFireBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarFireBomb/SolarFireBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarFireBomb/SolarFireBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(Tile tile) => true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Fire Morph Ball Bombs");
			Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Burns foes with the fury of the sun");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 150;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Red;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.OrangeTorch;
			dustType2 = DustID.SolarFlare;
			dustScale = 4f;
			dustScale2 = 2f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Daybreak, 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.FragmentSolar, 5)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
