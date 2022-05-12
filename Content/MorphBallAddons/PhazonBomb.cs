using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class PhazonBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PhazonBomb/PhazonBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PhazonBomb/PhazonBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PhazonBomb/PhazonBombProjectile";

		public override bool CanGenerateOnChozoStatue(Tile tile) => true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Morph Ball Bombs");
			Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Inflicts enemies with Phazon radiation poisoning");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 77;
			item.value = Terraria.Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.BlueCrystalShard;// These two ids are
			dustType2 = DustID.t_Crystal;//       the same thing.
			dustScale = 3f;
			dustScale2 = 2f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.PhazonDebuff>(), 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient<Items.Miscellaneous.PhazonBar>(5)
				.AddTile<Tiles.NovaWorkTableTile>()
				.Register();
		}
	}
}
