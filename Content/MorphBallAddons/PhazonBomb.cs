using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class PhazonBomb : Bomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PhazonBomb/PhazonBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PhazonBomb/PhazonBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PhazonBomb/PhazonBombProjectile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || NPC.downedPlantBoss
				? 1
				: 0
				;
		}

		public override void ItemSetDefaults()
		{
			Item.damage = 103;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Cyan;
		}

		public override void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.BlueCrystalShard;// These two ids are
			dustType2 = DustID.t_Crystal;//       the same thing.
			dustScale = 3f;
			dustScale2 = 2f;
		}

		public override void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.PhazonDebuff>(), 600);
		}

		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				//.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient<Items.Miscellaneous.PhazonBar>(5)
				.AddTile<Tiles.NovaWorkTableTile>()
				.Register();
		}
	}
}
